// Scripts/Player/WolfCharacter.cs
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class WolfCharacter : AnimalCharacter
{
    private PlayerControls controls;
    // cached callback so we can unsubscribe cleanly
    private System.Action<UnityEngine.InputSystem.InputAction.CallbackContext> chargePerformed;

    [Header("Wolf Abilities")]
    [SerializeField] private float chargeSpeed = 25f; // Швидкість ривка
    [SerializeField] private float chargeDuration = 0.3f; // Тривалість ривка
    [SerializeField] private int chargeDamage = 40; // Шкода від ривка
    [SerializeField] private float chargeCooldown = 8f;
    [SerializeField] private float chargeEnergyCost = 35f;

    private bool isCharging = false;
    private bool isChargeOnCooldown = false;
    private Vector2 moveInput;
    private Vector2 lookInput;
    
    // Ми використовуємо Awake для ініціалізації
    private void Awake()
    {
        controls = new PlayerControls();
    }
    
    private void OnEnable()
    {
        // prepare cached delegate and subscribe
        chargePerformed = ctx => TryActivateCharge();
        controls.InGame.Ability.performed += chargePerformed;
        controls.InGame.Enable();
    }

    private void OnDisable()
    {
        // unsubscribe to avoid duplicate callbacks
        if (chargePerformed != null)
            controls.InGame.Ability.performed -= chargePerformed;
        controls.InGame.Disable();
    }

    private void Update()
    {
        // Не дозволяємо зчитувати рух, поки персонаж робить ривок
        if (isCharging)
        {
            moveInput = Vector2.zero;
            return;
        }

        moveInput = controls.InGame.Move.ReadValue<Vector2>();
        lookInput = controls.InGame.Look.ReadValue<Vector2>();
        
        HandleRotation();
    }

    private void FixedUpdate()
    {
        // Якщо ми не в ривку, рухаємось як зазвичай
        if (!isCharging)
        {
            rb.linearVelocity = moveInput.normalized * moveSpeed;
        }
    }

    private void HandleRotation()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(lookInput);
        Vector2 direction = (mouseWorldPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle - 90f;
    }

    private void TryActivateCharge()
    {
        if (!isChargeOnCooldown && GetCurrentEnergy() >= chargeEnergyCost)
        {
            UseEnergy(chargeEnergyCost);
            StartCoroutine(ChargeRoutine());
        }
    }

    private IEnumerator ChargeRoutine()
    {
        isCharging = true;
        isChargeOnCooldown = true;
        
        // Ривок у напрямку, куди дивиться персонаж (transform.right)
        Vector2 chargeDirection = transform.right;
        rb.linearVelocity = chargeDirection * chargeSpeed;

        Debug.Log("WOLF CHARGE!");

        yield return new WaitForSeconds(chargeDuration);

        isCharging = false;
        rb.linearVelocity = Vector2.zero; // Зупиняємось після ривка

        yield return new WaitForSeconds(chargeCooldown);
        isChargeOnCooldown = false;
        Debug.Log("Charge is ready again.");
    }
    
    // Обробка зіткнення під час ривка
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isCharging) return;

        // Перевіряємо, чи це бос
        if (collision.gameObject.CompareTag("Boss"))
        {
            KolobokController boss = collision.gameObject.GetComponent<KolobokController>();
            if (boss != null)
            {
                Debug.Log("Charge hit the Kolobok!");
                boss.TakeDamage(chargeDamage);
                boss.GetStuckInTrap(); // Використовуємо ту ж логіку оглушення, що й для пастки
            }
        }
        // НОВИЙ КОД: Перевіряємо, чи не врізались ми в іншого гравця (для майбутнього кооперативу)
        else if (collision.gameObject.CompareTag("Player"))
        {
            AnimalCharacter otherPlayer = collision.gameObject.GetComponent<AnimalCharacter>();
            if (otherPlayer != null)
            {
                Debug.Log("Wolf charge hit another player!");
                otherPlayer.TakeDamage(chargeDamage);
                // Опціонально: відштовхування або інші ефекти тут
            }
        }

        // Завершуємо ривок після зіткнення з будь-яким твердим об'єктом
        isCharging = false;
        rb.linearVelocity = Vector2.zero;
    }
}