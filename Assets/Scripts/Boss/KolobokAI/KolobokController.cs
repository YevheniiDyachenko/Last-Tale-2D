// Scripts/Boss/KolobokController.cs
using UnityEngine;
using System.Collections;

/// <summary>
/// Manages the behavior and state of the Kolobok boss character.
/// </summary>
public class KolobokController : MonoBehaviour
{
    /// <summary>
    /// Defines the possible states of the Kolobok boss.
    /// </summary>
    public enum BossState {
        /// <summary>The boss is in the rolling stage.</summary>
        Stage1_Rolling,
        /// <summary>The boss is in the jumping stage.</summary>
        Stage2_Jumping,
        /// <summary>The boss is stuck in a trap.</summary>
        StuckInTrap,
        /// <summary>The boss is defeated.</summary>
        Defeated
    }

    /// <summary>
    /// The current state of the boss.
    /// </summary>
    public BossState currentState;
    private BossState previousState; // Зберігаємо попередній стан, щоб повернутись до нього

    [Header("Base Stats")]
    [SerializeField] private int maxHealth = 1000;
    [SerializeField] private float moveSpeed = 3f;

    [Header("Stage Settings")]
    [SerializeField] private float stage2HealthThreshold = 0.7f; // Перехід на 2 стадію при 70% здоров'я
    [SerializeField] private float jumpAttackCooldown = 5f; // Перезарядка стрибка
    [SerializeField] private float trapStunDuration = 2f; // НОВИЙ ПАРАМЕТР: тривалість застрягання

    private int currentHealth;
    private Transform target;
    private Rigidbody2D rb;
    private bool canAttack = true;
    private UIManager uiManager; // Посилання на UI Manager
    [Header("Attack Stats")]
    [SerializeField] private int touchDamage = 20; // Шкода від простого дотику

    /// <summary>
    /// Initializes the Kolobok boss.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        currentState = BossState.Stage1_Rolling; // Починаємо з першої стадії

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }

        // Знаходимо UI Manager та сповіщаємо про появу
    uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.ShowAnnouncement("КОЛОБОК З'ЯВИВСЯ!", 3f);
            uiManager.ShowBossUI(maxHealth);
        }
    }

    /// <summary>
    /// Called every fixed frame-rate frame. Handles the boss's movement and behavior.
    /// </summary>
    void FixedUpdate()
    {
        if (target == null || currentState == BossState.Defeated)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // НОВА ЛОГІКА: Перевіряємо, чи бачимо ми ціль
        if (!IsTargetVisible())
        {
            rb.linearVelocity = Vector2.zero;
            Debug.Log("Kolobok lost the target...");
            return;
        }

        // Логіка поведінки залежить від поточного стану
        switch (currentState)
        {
            case BossState.Stage1_Rolling:
                HandleRollingMovement();
                break;
            case BossState.Stage2_Jumping:
                HandleJumpingMovement();
                break;
            case BossState.StuckInTrap:
                // Коли застряг - нічого не робимо
                rb.linearVelocity = Vector2.zero;
                break;
        }
    }

    // НОВИЙ МЕТОД для перевірки видимості
    private bool IsTargetVisible()
    {
        // Перевіряємо, чи об'єкт цілі не знаходиться на шарі "Ignore Raycast"
        return target.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast");
    }

    /// <summary>
    /// Makes the boss get stuck in a trap for a specified duration.
    /// </summary>
    public void GetStuckInTrap()
    {
        StartCoroutine(StuckInTrapRoutine());
    }

    private IEnumerator StuckInTrapRoutine()
    {
        previousState = currentState; // Зберігаємо поточний стан
        currentState = BossState.StuckInTrap;
        Debug.Log("Kolobok is stuck in a trap!");

        yield return new WaitForSeconds(trapStunDuration);

        currentState = previousState; // Повертаємось до попереднього стану
        Debug.Log("Kolobok broke free!");
    }

    // --- Логіка стадій ---

    private void HandleRollingMovement()
    {
        // Просто котиться в напрямку гравця
    Vector2 direction = (target.position - transform.position).normalized;
    rb.linearVelocity = direction * moveSpeed;
    }

    private void HandleJumpingMovement()
    {
        // Зупиняється, готується до стрибка, стрибає, потім знову рухається
        if (canAttack)
        {
            StartCoroutine(JumpAttackRoutine());
        }
        else
        {
            // Можна додати рух між атаками, якщо потрібно
             Vector2 direction = (target.position - transform.position).normalized;
             rb.linearVelocity = direction * moveSpeed * 0.8f; // Рухається трохи повільніше
        }
    }

    private IEnumerator JumpAttackRoutine()
    {
    canAttack = false;
    rb.linearVelocity = Vector2.zero; // Зупиняємось

        Debug.Log("Kolobok is preparing to jump!");
        yield return new WaitForSeconds(1f); // Затримка перед стрибком

        // Симулюємо стрибок та удар по землі (тут можна додати анімацію та ефекти)
        Debug.Log("KOLOBOK JUMP ATTACK! (AoE Shockwave)");
        // Тут буде код для створення зони шкоди (shockwave)

        yield return new WaitForSeconds(jumpAttackCooldown);
        canAttack = true;
    }

    // --- Система здоров'я та стадій ---

    /// <summary>
    /// Applies damage to the boss.
    /// </summary>
    /// <param name="damage">The amount of damage to apply.</param>
    public void TakeDamage(int damage)
    {
        if (currentState == BossState.Defeated) return;

        currentHealth -= damage;
        Debug.Log($"Kolobok Health: {currentHealth}");

        // Оновлюємо UI при отриманні шкоди
        uiManager?.UpdateBossHealthBar(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            CheckStageTransition();
        }
    }

    private void CheckStageTransition()
    {
        // Перехід на другу стадію
        if (currentState == BossState.Stage1_Rolling && currentHealth <= maxHealth * stage2HealthThreshold)
        {
            TransitionToStage2();
        }
    }

    private void TransitionToStage2()
    {
        currentState = BossState.Stage2_Jumping;
        moveSpeed *= 1.2f; // Трохи збільшуємо швидкість
        Debug.Log("KOLOBOK ENTERS STAGE 2! It starts jumping!");
    }

    private void Die()
    {
        currentState = BossState.Defeated;
        Debug.Log("Kolobok is defeated!");
        // Ховаємо UI, коли бос переможений
        uiManager?.HideBossUI();
        // Тут буде логіка перемоги
        Destroy(gameObject, 2f); // Знищуємо об'єкт через 2 секунди
    }

    /// <summary>
    /// Called when a collision occurs.
    /// </summary>
    /// <param name="collision">The collision data.</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Перевіряємо, чи це гравець
        if (collision.gameObject.CompareTag("Player"))
        {
            AnimalCharacter player = collision.gameObject.GetComponent<AnimalCharacter>();
            if (player != null)
            {
                // Завдаємо шкоди гравцеві
                Debug.Log("Kolobok damaged the player!");
                player.TakeDamage(touchDamage);
            }
        }
    }
}