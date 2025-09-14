using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;


/// <summary>
/// Обробляє здатність гравця ставити пастки та використовувати їх посилення.
/// </summary>
[RequireComponent(typeof(AnimalCharacter))]
public class PlayerTrapAbility : MonoBehaviour
{
    [Header("Trap Placing")]
    [SerializeField] private GameObject trapPrefab;
    [SerializeField] private int trapCost = 1;

    [Header("Resources")]
    private int currentResources = 0;
    private UIManager uiManager;

    [Header("Trap Boost Ability")]
    [SerializeField] private float trapBoostDuration = 10f;
    [SerializeField] private float trapBoostCooldown = 25f;
    [SerializeField] private float trapBoostEnergyCost = 30f;

    /// <summary>
    /// Показує, чи активне на даний момент посилення пасток.
    /// </summary>
    public static bool isTrapBoostActive = false;

    private PlayerControls controls;
    private AnimalCharacter character;
    private bool isTrapBoostOnCooldown = false;


    /// <summary>
    /// Ініціалізує здатність ставити пастки.
    /// </summary>
    private void Awake()
    {
        controls = new PlayerControls();
        character = GetComponent<AnimalCharacter>();
        controls.InGame.PlaceTrap.performed += _ => TryPlaceTrap();
        controls.InGame.TrapBoost.performed += _ => TryActivateTrapBoost();
    }

    /// <summary>
    /// Ініціалізує UI для здатності ставити пастки.
    /// </summary>
    private void Start()
    {
        uiManager = FindFirstObjectByType<UIManager>();
        uiManager?.UpdateResourceText(currentResources);
    }

    /// <summary>
    /// Вмикає керування здатністю ставити пастки.
    /// </summary>
    private void OnEnable() => controls.InGame.Enable();

    /// <summary>
    /// Вимикає керування здатністю ставити пастки.
    /// </summary>
    private void OnDisable() => controls.InGame.Disable();

    private void TryPlaceTrap()
    {
        if (trapPrefab != null && currentResources >= trapCost)
        {
            currentResources -= trapCost;
            uiManager?.UpdateResourceText(currentResources);
            Instantiate(trapPrefab, transform.position, Quaternion.identity);
            Debug.Log($"Trap placed! Resources left: {currentResources}");
        }
        else
        {
            Debug.Log("Not enough resources to place a trap!");
        }
    }

    /// <summary>
    /// Викликається, коли інший колайдер входить у тригер.
    /// </summary>
    /// <param name="other">Інший колайдер.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Resource"))
        {
            CollectibleResource resource = other.GetComponent<CollectibleResource>();
            if (resource != null)
            {
                currentResources += resource.amount;
                uiManager?.UpdateResourceText(currentResources);
                Debug.Log($"Collected {resource.amount} resource(s)! Total: {currentResources}");
                Destroy(other.gameObject);
            }
        }
    }

    /// <summary>
    /// Спроба активувати посилення пасток.
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
