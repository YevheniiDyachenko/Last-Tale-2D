using UnityEngine;

// Scripts/Player/AnimalCharacter.cs
public abstract class AnimalCharacter : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] protected float moveSpeed = 8f;
    [SerializeField] protected float maxHealth = 100f; // Додаємо здоров'я
    [SerializeField] protected float maxEnergy = 100f;

    protected float currentHealth; // Додаємо поточне здоров'я
    protected float currentEnergy;
    protected Rigidbody2D rb;

    private UIManager uiManager; // Посилання на UI Manager

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;

        // Знаходимо UI Manager на сцені
    uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager != null)
        {
            // Ініціалізуємо смужки при старті
            uiManager.InitHealthBar(maxHealth);
            uiManager.InitEnergyBar(maxEnergy);
        }
    }

    // --- Методи для роботи з енергією ---
    public float GetCurrentEnergy() => currentEnergy;

    public void UseEnergy(float amount)
    {
        currentEnergy -= amount;
        uiManager?.UpdateEnergyBar(currentEnergy); // Оновлюємо UI
    }

    // --- НОВІ МЕТОДИ для роботи зі здоров'ям ---
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        uiManager?.UpdateHealthBar(currentHealth); // Оновлюємо UI
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");

        // Показуємо екран смерті
        uiManager?.ShowDeathScreen();

        // Повідомляємо GameManager про смерть гравця
        GameManager.Instance.PlayerDied();
        
        // Вимикаємо керування та фізику, щоб персонаж "завмер"
        this.enabled = false; // Вимикаємо поточний скрипт (FoxCharacter або WolfCharacter)
        
        // Вимикаємо компоненти безпечно
        var attack = GetComponent<PlayerAttack>();
        if (attack != null) attack.enabled = false;
        
        var trapAbility = GetComponent<PlayerTrapAbility>();
        if (trapAbility != null) trapAbility.enabled = false;
        
        // Вимикаємо фізику
        rb.bodyType = RigidbodyType2D.Kinematic; // Замість застарілого isKinematic
        rb.linearVelocity = Vector2.zero; // Замість застарілого velocity
    }
}
