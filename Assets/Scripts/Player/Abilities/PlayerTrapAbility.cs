using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;


/// <summary>
/// Handles the player's ability to place traps and use a trap boost.
/// </summary>
[RequireComponent(typeof(AnimalCharacter))]
public class PlayerTrapAbility : MonoBehaviour
{
    [Header("Trap Placing")]
    [SerializeField] private GameObject trapPrefab;
    [SerializeField] private int trapCost = 1; // Вартість пастки в ресурсах

    [Header("Resources")]
    private int currentResources = 0; // Поточна кількість ресурсів
    private UIManager uiManager; // Посилання на UI Manager

    [Header("Trap Boost Ability")]
    [SerializeField] private float trapBoostDuration = 10f;
    [SerializeField] private float trapBoostCooldown = 25f;
    [SerializeField] private float trapBoostEnergyCost = 30f;
    /// <summary>
    /// Indicates whether the trap boost is currently active.
    /// </summary>
    public static bool isTrapBoostActive = false;

    private PlayerControls controls;
    private AnimalCharacter character;
    private bool isTrapBoostOnCooldown = false;


    /// <summary>
    /// Initializes the trap ability.
    /// </summary>
    private void Awake()
    {
        controls = new PlayerControls();
        character = GetComponent<AnimalCharacter>();
        controls.InGame.PlaceTrap.performed += _ => TryPlaceTrap();
        controls.InGame.TrapBoost.performed += _ => TryActivateTrapBoost();
    }

    /// <summary>
    /// Initializes the UI for the trap ability.
    /// </summary>
    private void Start()
    {
        // Знаходимо UI Manager та ініціалізуємо текст ресурсів
        uiManager = FindFirstObjectByType<UIManager>();
        uiManager?.UpdateResourceText(currentResources);
    }

    /// <summary>
    /// Enables the trap ability controls.
    /// </summary>
    private void OnEnable() => controls.InGame.Enable();

    /// <summary>
    /// Disables the trap ability controls.
    /// </summary>
    private void OnDisable() => controls.InGame.Disable();

    private void TryPlaceTrap()
    {
        if (trapPrefab != null && currentResources >= trapCost)
        {
            currentResources -= trapCost;
            uiManager?.UpdateResourceText(currentResources); // Оновлюємо UI
            Instantiate(trapPrefab, transform.position, Quaternion.identity);
            Debug.Log($"Trap placed! Resources left: {currentResources}");
        }
        else
        {
            Debug.Log("Not enough resources to place a trap!");
        }
    }

    /// <summary>
    /// Called when another collider enters the trigger.
    /// </summary>
    /// <param name="other">The other collider.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Resource"))
        {
            CollectibleResource resource = other.GetComponent<CollectibleResource>();
            if (resource != null)
            {
                currentResources += resource.amount;
                uiManager?.UpdateResourceText(currentResources); // Оновлюємо UI
                Debug.Log($"Collected {resource.amount} resource(s)! Total: {currentResources}");
                Destroy(other.gameObject);
            }
        }
    }

    /// <summary>
    /// Attempts to activate the trap boost ability.
    /// </summary>
    private void TryActivateTrapBoost()
    {
        if (!isTrapBoostOnCooldown && character.GetCurrentEnergy() >= trapBoostEnergyCost)
        {
            character.UseEnergy(trapBoostEnergyCost);
            StartCoroutine(ActivateTrapBoost());
        }
    }

    private IEnumerator ActivateTrapBoost()
    {
        isTrapBoostOnCooldown = true;
        isTrapBoostActive = true;
        Debug.Log("TRAP BOOST ACTIVATED!");

        yield return new WaitForSeconds(trapBoostDuration);

        isTrapBoostActive = false;
        Debug.Log("Trap boost finished.");

        yield return new WaitForSeconds(trapBoostCooldown);

        isTrapBoostOnCooldown = false;
        Debug.Log("Trap boost is ready again.");
    }
}
