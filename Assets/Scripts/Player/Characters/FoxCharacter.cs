using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

/// <summary>
/// Представляє персонажа Лисицю зі здатністю невидимості.
/// </summary>
public class FoxCharacter : AnimalCharacter
{
    private PlayerControls controls;

    [Header("Fox Abilities")]
    /// <summary>
    /// Тривалість невидимості.
    /// </summary>
    [SerializeField] private float stealthDuration = 5f;
    /// <summary>
    /// Перезарядка невидимості.
    /// </summary>
    [SerializeField] private float stealthCooldown = 20f;
    /// <summary>
    /// Вартість невидимості в енергії.
    /// </summary>
    [SerializeField] private float stealthEnergyCost = 40f;

    private bool isStealthOnCooldown = false;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveInput;
    private Vector2 lookInput;

    /// <summary>
    /// Ініціалізує персонажа Лисицю.
    /// </summary>
    private void Awake()
    {
        controls = new PlayerControls();
        spriteRenderer = GetComponent<SpriteRenderer>();
        controls.InGame.Ability.performed += _ => TryActivateStealth();
    }

    /// <summary>
    /// Вмикає керування персонажем.
    /// </summary>
    private void OnEnable()
    {
        controls.InGame.Enable();
    }

    /// <summary>
    /// Вимикає керування персонажем.
    /// </summary>
    private void OnDisable()
    {
        controls.InGame.Disable();
    }

    /// <summary>
    /// Ініціалізує базові характеристики персонажа.
    /// </summary>
    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// Викликається щокадру. Обробляє ввід гравця та обертання.
    /// </summary>
    private void Update()
    {
        moveInput = controls.InGame.Move.ReadValue<Vector2>();
        lookInput = controls.InGame.Look.ReadValue<Vector2>();
        HandleRotation();
    }

    /// <summary>
    /// Викликається кожен фіксований кадр. Обробляє рух персонажа.
    /// </summary>
    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput.normalized * moveSpeed;
    }

    private void HandleRotation()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(lookInput);
        Vector2 direction = (mouseWorldPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle - 90f;
    }

    private void TryActivateStealth()
    {
        if (!isStealthOnCooldown && currentEnergy >= stealthEnergyCost)
        {
            StartCoroutine(ActivateStealth());
        }
    }

    private IEnumerator ActivateStealth()
    {
        isStealthOnCooldown = true;
        currentEnergy -= stealthEnergyCost;
        Debug.Log("STEALTH ACTIVATED!");
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        yield return new WaitForSeconds(stealthDuration);
        spriteRenderer.color = originalColor;
        gameObject.layer = LayerMask.NameToLayer("Default");
        Debug.Log("Stealth finished.");
        yield return new WaitForSeconds(stealthCooldown);
        isStealthOnCooldown = false;
        Debug.Log("Stealth is ready again.");
    }

}
