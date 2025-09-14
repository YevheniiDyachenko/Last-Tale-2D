// Scripts/Player/WolfCharacter.cs
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

/// <summary>
/// Представляє персонажа Вовка зі здатністю ривка.
/// </summary>
public class WolfCharacter : AnimalCharacter
{
    private PlayerControls controls;
    private System.Action<UnityEngine.InputSystem.InputAction.CallbackContext> chargePerformed;

    [Header("Wolf Abilities")]
    /// <summary>
    /// Швидкість ривка.
    /// </summary>
    [SerializeField] private float chargeSpeed = 25f;
    /// <summary>
    /// Тривалість ривка.
    /// </summary>
    [SerializeField] private float chargeDuration = 0.3f;
    /// <summary>
    /// Шкода від ривка.
    /// </summary>
    [SerializeField] private int chargeDamage = 40;
    /// <summary>
    /// Перезарядка ривка.
    /// </summary>
    [SerializeField] private float chargeCooldown = 8f;
    /// <summary>
    /// Вартість ривка в енергії.
    /// </summary>
    [SerializeField] private float chargeEnergyCost = 35f;

    private bool isCharging = false;
    private bool isChargeOnCooldown = false;
    private Vector2 moveInput;
    private Vector2 lookInput;
    
    /// <summary>
    /// Ініціалізує персонажа Вовка.
    /// </summary>
    private void Awake()
    {
        controls = new PlayerControls();
    }
    
    /// <summary>
    /// Вмикає керування персонажем.
    /// </summary>
    private void OnEnable()
    {
        chargePerformed = ctx => TryActivateCharge();
        controls.InGame.Ability.performed += chargePerformed;
        controls.InGame.Enable();
    }

    /// <summary>
    /// Вимикає керування персонажем.
    /// </summary>
    private void OnDisable()
    {
        if (chargePerformed != null)
            controls.InGame.Ability.performed -= chargePerformed;
        controls.InGame.Disable();
    }

    /// <summary>
    /// Викликається щокадру. Обробляє ввід гравця та обертання.
    /// </summary>
    private void Update()
    {
        if (isCharging)
        {
            moveInput = Vector2.zero;
            return;
        }

        moveInput = controls.InGame.Move.ReadValue<Vector2>();
        lookInput = controls.InGame.Look.ReadValue<Vector2>();
        
        HandleRotation();
    }

    /// <summary>
    /// Викликається кожен фіксований кадр. Обробляє рух персонажа.
    /// </summary>
    private void FixedUpdate()
    {
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
        
        Vector2 chargeDirection = transform.right;
        rb.linearVelocity = chargeDirection * chargeSpeed;

        Debug.Log("WOLF CHARGE!");

        yield return new WaitForSeconds(chargeDuration);

        isCharging = false;
        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(chargeCooldown);
        isChargeOnCooldown = false;
        Debug.Log("Charge is ready again.");
    }
    
    /// <summary>
    /// Викликається при зіткненні під час ривка.
    /// </summary>
    /// <param name="collision">Дані про зіткнення.</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isCharging) return;

        if (collision.gameObject.CompareTag("Boss"))
        {
            KolobokController boss = collision.gameObject.GetComponent<KolobokController>();
            if (boss != null)
            {
                Debug.Log("Charge hit the Kolobok!");
                boss.TakeDamage(chargeDamage);
                boss.GetStuckInTrap();
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            AnimalCharacter otherPlayer = collision.gameObject.GetComponent<AnimalCharacter>();
            if (otherPlayer != null)
            {
                Debug.Log("Wolf charge hit another player!");
                otherPlayer.TakeDamage(chargeDamage);
            }
        }

        isCharging = false;
        rb.linearVelocity = Vector2.zero;
    }
}