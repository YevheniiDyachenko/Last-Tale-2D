using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement; // Додаємо для роботи зі сценами

/// <summary>
/// Manages the user interface, including player stats, boss UI, and game over screen.
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider energyBar;
    [SerializeField] private TextMeshProUGUI resourceText;

    [Header("Boss UI")]
    [SerializeField] private GameObject bossUIPanel; // Посилання на всю панель боса
    [SerializeField] private Slider bossHealthBar;
    [SerializeField] private TextMeshProUGUI announcementText;

    [Header("Game Over UI")]
    [SerializeField] private GameObject deathScreenPanel; // Посилання на панель смерті
    [SerializeField] private CanvasGroup deathScreenCanvasGroup; // Для анімації прозорості
    [SerializeField] private TextMeshProUGUI deathScreenTitle; // Заголовок "GAME OVER"
    [SerializeField] private Button retryButton; // Кнопка "Спробувати знову"
    [SerializeField] private Button menuButton; // Кнопка "Вийти в меню"
    [SerializeField] private float fadeSpeed = 2f; // Швидкість появи/зникнення екрана

    // --- Методи для ініціалізації ---

    /// <summary>
    /// Initializes the health bar with the maximum health value.
    /// </summary>
    /// <param name="maxHealth">The maximum health of the player.</param>
    public void InitHealthBar(float maxHealth)
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
    }

    /// <summary>
    /// Initializes the energy bar with the maximum energy value.
    /// </summary>
    /// <param name="maxEnergy">The maximum energy of the player.</param>
    public void InitEnergyBar(float maxEnergy)
    {
        energyBar.maxValue = maxEnergy;
        energyBar.value = maxEnergy;
    }

    // --- Методи для оновлення ---

    /// <summary>
    /// Updates the health bar with the current health value.
    /// </summary>
    /// <param name="currentHealth">The current health of the player.</param>
    public void UpdateHealthBar(float currentHealth)
    {
        healthBar.value = currentHealth;
    }

    /// <summary>
    /// Updates the energy bar with the current energy value.
    /// </summary>
    /// <param name="currentEnergy">The current energy of the player.</param>
    public void UpdateEnergyBar(float currentEnergy)
    {
        energyBar.value = currentEnergy;
    }

    /// <summary>
    /// Updates the resource text with the current number of resources.
    /// </summary>
    /// <param name="currentResources">The current number of resources.</param>
    public void UpdateResourceText(int currentResources)
    {
        resourceText.text = $"Ресурси: {currentResources}";
    }

    // --- НОВІ МЕТОДИ для Боса ---

    /// <summary>
    /// Shows the boss UI and initializes the boss health bar.
    /// </summary>
    /// <param name="maxHealth">The maximum health of the boss.</param>
    public void ShowBossUI(float maxHealth)
    {
        bossUIPanel.SetActive(true);
        bossHealthBar.maxValue = maxHealth;
        bossHealthBar.value = maxHealth;
    }

    /// <summary>
    /// Hides the boss UI.
    /// </summary>
    public void HideBossUI()
    {
        bossUIPanel.SetActive(false);
    }

    /// <summary>
    /// Updates the boss health bar with the current health value.
    /// </summary>
    /// <param name="currentHealth">The current health of the boss.</param>
    public void UpdateBossHealthBar(float currentHealth)
    {
        bossHealthBar.value = currentHealth;
    }

    /// <summary>
    /// Shows an announcement message for a specified duration.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="duration">The duration to display the message.</param>
    public void ShowAnnouncement(string message, float duration)
    {
        StartCoroutine(AnnouncementRoutine(message, duration));
    }

    private IEnumerator AnnouncementRoutine(string message, float duration)
    {
        announcementText.text = message;
        announcementText.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        announcementText.gameObject.SetActive(false);
    }

    // --- Методи для екрана смерті ---
    private Coroutine currentDeathScreenAnimation;

    /// <summary>
    /// Shows the death screen with a fade-in animation.
    /// </summary>
    public void ShowDeathScreen()
    {
        // Зупиняємо попередню анімацію, якщо вона є
        if (currentDeathScreenAnimation != null)
            StopCoroutine(currentDeathScreenAnimation);

        // Ховаємо панель боса
        bossUIPanel?.SetActive(false);

        // Готуємо панель до показу
        deathScreenPanel.SetActive(true);
        deathScreenCanvasGroup.alpha = 0f;

        // Запускаємо анімацію появи
        currentDeathScreenAnimation = StartCoroutine(AnimateDeathScreen(1f));
    }

    /// <summary>
    /// Hides the death screen with a fade-out animation.
    /// </summary>
    public void HideDeathScreen()
    {
        // Зупиняємо попередню анімацію, якщо вона є
        if (currentDeathScreenAnimation != null)
            StopCoroutine(currentDeathScreenAnimation);

        // Запускаємо анімацію зникнення
        currentDeathScreenAnimation = StartCoroutine(AnimateDeathScreen(0f));
    }

    private IEnumerator AnimateDeathScreen(float targetAlpha)
    {
        // Перевіряємо, чи всі потрібні компоненти є
        if (deathScreenCanvasGroup == null)
        {
            Debug.LogError("Death Screen CanvasGroup is not assigned!");
            yield break;
        }

        float currentAlpha = deathScreenCanvasGroup.alpha;
        
        // Анімуємо alpha до цільового значення
        while (!Mathf.Approximately(currentAlpha, targetAlpha))
        {
            currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime);
            deathScreenCanvasGroup.alpha = currentAlpha;
            yield return null;
        }

        // Якщо сховали повністю - деактивуємо панель
        if (Mathf.Approximately(targetAlpha, 0f))
        {
            deathScreenPanel.SetActive(false);
        }

        currentDeathScreenAnimation = null;
    }

    // --- Методи роботи з кнопками ---
    /// <summary>
    /// Initializes the UI manager.
    /// </summary>
    private void Start()
    {
        // Переконуємося, що екран смерті спочатку схований
        if (deathScreenPanel != null)
            deathScreenPanel.SetActive(false);

        // Налаштовуємо кнопки
        if (retryButton != null)
            retryButton.onClick.AddListener(OnRetryButtonClick);
        
        if (menuButton != null)
            menuButton.onClick.AddListener(OnMenuButtonClick);
    }

    private void OnRetryButtonClick()
    {
        // Перезапускаємо поточну сцену
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    private void OnMenuButtonClick()
    {
        // Завантажуємо сцену головного меню
        SceneManager.LoadScene("MainMenu"); // Переконайтеся, що сцена MainMenu існує і додана в Build Settings
    }

    /// <summary>
    /// Cleans up the button listeners when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        // Видаляємо слухачів подій при знищенні об'єкта
        if (retryButton != null)
            retryButton.onClick.RemoveListener(OnRetryButtonClick);
        
        if (menuButton != null)
            menuButton.onClick.RemoveListener(OnMenuButtonClick);
    }
}
