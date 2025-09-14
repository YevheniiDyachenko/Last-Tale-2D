// Scripts/Boss/KolobokController.cs
using UnityEngine;
using System.Collections;

/// <summary>
/// Керує поведінкою та станом боса Колобка.
/// </summary>
public class KolobokController : MonoBehaviour
{
    /// <summary>
    /// Визначає можливі стани боса Колобка.
    /// </summary>
    public enum BossState {
        /// <summary>Бос знаходиться у стадії кочення.</summary>
        Stage1_Rolling,
        /// <summary>Бос знаходиться у стадії стрибків.</summary>
        Stage2_Jumping,
        /// <summary>Бос застряг у пастці.</summary>
        StuckInTrap,
        /// <summary>Бос переможений.</summary>
        Defeated
    }

    /// <summary>
    /// Поточний стан боса.
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
    /// <summary>
    /// Шкода, яку бос завдає при дотику.
    /// </summary>
    [SerializeField] private int touchDamage = 20; // Шкода від простого дотику

    /// <summary>
    /// Ініціалізує боса Колобка.
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
    /// Викликається кожен фіксований кадр. Керує рухом та поведінкою боса.
    /// </summary>
    void FixedUpdate()
    {
        if (target == null || currentState == BossState.Defeated)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (!IsTargetVisible())
        {
            rb.linearVelocity = Vector2.zero;
            Debug.Log("Kolobok lost the target...");
            return;
        }

        switch (currentState)
        {
            case BossState.Stage1_Rolling:
                HandleRollingMovement();
                break;
            case BossState.Stage2_Jumping:
                HandleJumpingMovement();
                break;
            case BossState.StuckInTrap:
                rb.linearVelocity = Vector2.zero;
                break;
        }
    }

    private bool IsTargetVisible()
    {
        return target.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast");
    }

    /// <summary>
    /// Змушує боса застрягти в пастці на певний час.
    /// </summary>
    public void GetStuckInTrap()
    {
        StartCoroutine(StuckInTrapRoutine());
    }

    private IEnumerator StuckInTrapRoutine()
    {
        previousState = currentState;
        currentState = BossState.StuckInTrap;
        Debug.Log("Kolobok is stuck in a trap!");

        yield return new WaitForSeconds(trapStunDuration);

        currentState = previousState;
        Debug.Log("Kolobok broke free!");
    }

    private void HandleRollingMovement()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    private void HandleJumpingMovement()
    {
        if (canAttack)
        {
            StartCoroutine(JumpAttackRoutine());
        }
        else
        {
             Vector2 direction = (target.position - transform.position).normalized;
             rb.linearVelocity = direction * moveSpeed * 0.8f;
        }
    }

    private IEnumerator JumpAttackRoutine()
    {
        canAttack = false;
        rb.linearVelocity = Vector2.zero;

        Debug.Log("Kolobok is preparing to jump!");
        yield return new WaitForSeconds(1f);

        Debug.Log("KOLOBOK JUMP ATTACK! (AoE Shockwave)");

        yield return new WaitForSeconds(jumpAttackCooldown);
        canAttack = true;
    }

    /// <summary>
    /// Завдає шкоди босу.
    /// </summary>
    /// <param name="damage">Кількість шкоди.</param>
    public void TakeDamage(int damage)
    {
        if (currentState == BossState.Defeated) return;

        currentHealth -= damage;
        Debug.Log($"Kolobok Health: {currentHealth}");

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
        if (currentState == BossState.Stage1_Rolling && currentHealth <= maxHealth * stage2HealthThreshold)
        {
            TransitionToStage2();
        }
    }

    private void TransitionToStage2()
    {
        currentState = BossState.Stage2_Jumping;
        moveSpeed *= 1.2f;
        Debug.Log("KOLOBOK ENTERS STAGE 2! It starts jumping!");
    }

    private void Die()
    {
        currentState = BossState.Defeated;
        Debug.Log("Kolobok is defeated!");
        uiManager?.HideBossUI();
        Destroy(gameObject, 2f);
    }

    /// <summary>
    /// Викликається при зіткненні.
    /// </summary>
    /// <param name="collision">Дані про зіткнення.</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AnimalCharacter player = collision.gameObject.GetComponent<AnimalCharacter>();
            if (player != null)
            {
                Debug.Log("Kolobok damaged the player!");
                player.TakeDamage(touchDamage);
            }
        }
    }
}