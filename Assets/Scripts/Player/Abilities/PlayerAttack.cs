using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

/// <summary>
/// Обробляє здатність гравця до атаки.
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    /// <summary>
    /// Шкода від атаки.
    /// </summary>
    [SerializeField] private int attackDamage = 15;
    /// <summary>
    /// Дальність атаки.
    /// </summary>
    [SerializeField] private float attackRange = 1.5f;
    /// <summary>
    /// Перезарядка атаки.
    /// </summary>
    [SerializeField] private float attackCooldown = 0.8f;
    /// <summary>
    /// Шар, на якому знаходиться бос.
    /// </summary>
    [SerializeField] private LayerMask bossLayer;

    private PlayerControls controls;
    private bool canAttack = true;

    /// <summary>
    /// Ініціалізує керування атакою гравця.
    /// </summary>
    private void Awake()
    {
        controls = new PlayerControls();
        controls.InGame.Attack.performed += _ => TryAttack();
    }

    /// <summary>
    /// Вмикає керування атакою.
    /// </summary>
    private void OnEnable() => controls.InGame.Enable();

    /// <summary>
    /// Вимикає керування атакою.
    /// </summary>
    private void OnDisable() => controls.InGame.Disable();

    private void TryAttack()
    {
        if (canAttack)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        canAttack = false;
        Debug.Log("Player attacks!");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position + transform.right * (attackRange / 2), attackRange, bossLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            KolobokController boss = enemy.GetComponent<KolobokController>();
            if (boss != null)
            {
                Debug.Log("Attack hit Kolobok!");
                boss.TakeDamage(attackDamage);
            }
        }

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    /// <summary>
    /// Малює гизмо для візуалізації дальності атаки.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.right * (attackRange / 2), attackRange);
    }
}
