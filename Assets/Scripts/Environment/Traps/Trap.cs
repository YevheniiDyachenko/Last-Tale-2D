// Scripts/Environment/Trap.cs
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))] // Переконуємось, що на об'єкті є SpriteRenderer
public class Trap : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int baseDamage = 50;
    [SerializeField] private float boostMultiplier = 2f;
    [SerializeField] private Color boostedColor = Color.blue; // Колір для підсиленої пастки

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        // Отримуємо компонент один раз, щоб не шукати його постійно
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // НОВИЙ КОД: Перевіряємо, чи активне підсилення, при появі пастки
        if (PlayerTrapAbility.isTrapBoostActive)
        {
            // Якщо так, змінюємо колір
            spriteRenderer.color = boostedColor;
            Debug.Log("A boosted (blue) trap has been placed!");
        }
    }

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