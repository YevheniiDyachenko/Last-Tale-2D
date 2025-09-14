using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

/// <summary>
/// Handles the player's attack ability.
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private int attackDamage = 15;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 0.8f;
    [SerializeField] private LayerMask bossLayer;

    private PlayerControls controls;
    private bool canAttack = true;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.InGame.Attack.performed += _ => TryAttack();
    }

    private void OnEnable() => controls.InGame.Enable();
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

        // Атака у напрямку transform.right (вперед)
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.right * (attackRange / 2), attackRange);
    }
}
