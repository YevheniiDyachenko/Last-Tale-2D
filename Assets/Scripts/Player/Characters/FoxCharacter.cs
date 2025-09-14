
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class FoxCharacter : AnimalCharacter
{
    private PlayerControls controls;

    [Header("Fox Abilities")]
    [SerializeField] private float stealthDuration = 5f;
    [SerializeField] private float stealthCooldown = 20f;
    [SerializeField] private float stealthEnergyCost = 40f;

    private bool isStealthOnCooldown = false;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveInput;
    private Vector2 lookInput;

    private void Awake()
    {
        controls = new PlayerControls();
        spriteRenderer = GetComponent<SpriteRenderer>();
        controls.InGame.Ability.performed += _ => TryActivateStealth();
    }

    private void OnEnable()
    {
        controls.InGame.Enable();
    }

    private void OnDisable()
    {
        controls.InGame.Disable();
    }

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        moveInput = controls.InGame.Move.ReadValue<Vector2>();
        lookInput = controls.InGame.Look.ReadValue<Vector2>();
        HandleRotation();
    }

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
