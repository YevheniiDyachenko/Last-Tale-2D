using UnityEngine;

// Scripts/Player/AnimalCharacter.cs
/// <summary>
/// Абстрактний базовий клас для всіх персонажів-тварин у грі.
/// </summary>
public abstract class AnimalCharacter : MonoBehaviour
{
    [Header("Base Stats")]
    /// <summary>
    /// Швидкість руху персонажа.
    /// </summary>
    [SerializeField] protected float moveSpeed = 8f;
    /// <summary>
    /// Максимальне здоров'я персонажа.
    /// </summary>
    [SerializeField] protected float maxHealth = 100f;
    /// <summary>
    /// Максимальна енергія персонажа.
    /// </summary>
    [SerializeField] protected float maxEnergy = 100f;

    protected float currentHealth;
    protected float currentEnergy;
    protected Rigidbody2D rb;

    private UIManager uiManager;

    /// <summary>
    /// Ініціалізує характеристики персонажа та UI.
    /// </summary>
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;

        uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.InitHealthBar(maxHealth);
            uiManager.InitEnergyBar(maxEnergy);
        }
    }

    /// <summary>
    /// Повертає поточну енергію персонажа.
    /// </summary>
    /// <returns>Поточна енергія.</returns>
    public float GetCurrentEnergy() => currentEnergy;

    /// <summary>
    /// Зменшує енергію персонажа на вказану кількість.
    /// </summary>
    /// <param name="amount">Кількість енергії, яку потрібно використати.</param>
    public void UseEnergy(float amount)
    {
        currentEnergy -= amount;
        uiManager?.UpdateEnergyBar(currentEnergy);
    }

    /// <summary>
    /// Завдає шкоди персонажу.
    /// </summary>
    /// <param name="amount">Кількість шкоди.</param>
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        uiManager?.UpdateHealthBar(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");

        uiManager?.ShowDeathScreen();

        GameManager.Instance.PlayerDied();
        
        this.enabled = false;
        
        var attack = GetComponent<PlayerAttack>();
        if (attack != null) attack.enabled = false;
        
        var trapAbility = GetComponent<PlayerTrapAbility>();
        if (trapAbility != null) trapAbility.enabled = false;
        
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
    }
}
