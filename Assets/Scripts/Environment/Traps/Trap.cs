// Scripts/Environment/Trap.cs
using UnityEngine;

/// <summary>
/// Представляє пастку, яку можна розмістити в ігровому світі.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class Trap : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int baseDamage = 50;
    [SerializeField] private float boostMultiplier = 2f;
    [SerializeField] private Color boostedColor = Color.blue;

    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Ініціалізує пастку.
    /// </summary>
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Викликається при створенні пастки.
    /// </summary>
    private void Start()
    {
        if (PlayerTrapAbility.isTrapBoostActive)
        {
            spriteRenderer.color = boostedColor;
            Debug.Log("A boosted (blue) trap has been placed!");
        }
    }

    /// <summary>
    /// Викликається, коли інший колайдер входить у тригер пастки.
    /// </summary>
    /// <param name="other">Інший колайдер.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            KolobokController boss = other.GetComponent<KolobokController>();
            if (boss != null)
            {
                int finalDamage = baseDamage;
                if (PlayerTrapAbility.isTrapBoostActive)
                {
                    finalDamage = (int)(baseDamage * boostMultiplier);
                    PlayerTrapAbility.isTrapBoostActive = false;
                    Debug.Log("Boosted trap triggered!");
                }
                boss.TakeDamage(finalDamage);
                boss.GetStuckInTrap();
            }
            Destroy(gameObject);
        }
    }
}