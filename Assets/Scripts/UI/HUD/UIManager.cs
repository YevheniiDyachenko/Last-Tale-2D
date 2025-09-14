using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Керує інтерфейсом користувача, включаючи статистику гравця, UI боса та екран завершення гри.
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("Player Stats")]
    /// <summary>
    /// Слайдер здоров'я гравця.
    /// </summary>
    [SerializeField] private Slider healthBar;
    /// <summary>
    /// Слайдер енергії гравця.
    /// </summary>
    [SerializeField] private Slider energyBar;
    /// <summary>
    /// Текст для відображення кількості ресурсів.
    /// </summary>
    [SerializeField] private TextMeshProUGUI resourceText;

    [Header("Boss UI")]
    /// <summary>
    /// Панель UI боса.
    /// </summary>
    [SerializeField] private GameObject bossUIPanel;
    /// <summary>
    /// Слайдер здоров'я боса.
    /// </summary>
    [SerializeField] private Slider bossHealthBar;
    /// <summary>
    /// Текст для анонсів.
    /// </summary>
    [SerializeField] private TextMeshProUGUI announcementText;

    [Header("Game Over UI")]
    /// <summary>
    /// Панель екрана смерті.
    /// </summary>
    [SerializeField] private GameObject deathScreenPanel;
    /// <summary>
    /// CanvasGroup для анімації прозорості екрана смерті.
    /// </summary>
    [SerializeField] private CanvasGroup deathScreenCanvasGroup;
    /// <summary>
    /// Заголовок на екрані смерті.
    /// </summary>
    [SerializeField] private TextMeshProUGUI deathScreenTitle;
    /// <summary>
    /// Кнопка "Спробувати знову".
    /// </summary>
    [SerializeField] private Button retryButton;
    /// <summary>
    /// Кнопка "Вийти в меню".
    /// </summary>
    [SerializeField] private Button menuButton;
    /// <summary>
    /// Швидкість анімації появи/зникнення екрана смерті.
    /// </summary>
    [SerializeField] private float fadeSpeed = 2f;

    /// <summary>
    /// Ініціалізує смугу здоров'я з максимальним значенням.
    /// </summary>
    /// <param name="maxHealth">Максимальне здоров'я гравця.</param>
    public void InitHealthBar(float maxHealth)
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
    }

    /// <summary>
    /// Ініціалізує смугу енергії з максимальним значенням.
    /// </summary>
    /// <param name="maxEnergy">Максимальна енергія гравця.</param>
    public void InitEnergyBar(float maxEnergy)
    {
        energyBar.maxValue = maxEnergy;
        energyBar.value = maxEnergy;
    }

    /// <summary>
    /// Оновлює смугу здоров'я поточним значенням.
    /// </summary>
    /// <param name="currentHealth">Поточне здоров'я гравця.</param>
    public void UpdateHealthBar(float currentHealth)
    {
        healthBar.value = currentHealth;
    }

    /// <summary>
    /// Оновлює смугу енергії поточним значенням.
    /// </summary>
    /// <param name="currentEnergy">Поточна енергія гравця.</param>
    public void UpdateEnergyBar(float currentEnergy)
    {
        energyBar.value = currentEnergy;
    }

    /// <summary>
    /// Оновлює текст ресурсів поточною кількістю.
    /// </summary>
    /// <param name="currentResources">Поточна кількість ресурсів.</param>
    public void UpdateResourceText(int currentResources)
    {
        resourceText.text = $"Ресурси: {currentResources}";
    }

    /// <summary>
    /// Показує UI боса та ініціалізує його смугу здоров'я.
    /// </summary>
    /// <param name="maxHealth">Максимальне здоров'я боса.</param>
    public void ShowBossUI(float maxHealth)
    {
        bossUIPanel.SetActive(true);
        bossHealthBar.maxValue = maxHealth;
        bossHealthBar.value = maxHealth;
    }

    /// <summary>
    /// Ховає UI боса.
    /// </summary>
    public void HideBossUI()
    {
        bossUIPanel.SetActive(false);
    }

    /// <summary>
    /// Оновлює смугу здоров'я боса поточним значенням.
    /// </summary>
    /// <param name="currentHealth">Поточне здоров'я боса.</param>
    public void UpdateBossHealthBar(float currentHealth)
    {
        bossHealthBar.value = currentHealth;
    }

    /// <summary>
    /// Показує повідомлення на екрані на певний час.
    /// </summary>
    /// <param name="message">Повідомлення для відображення.</param>
    /// <param name="duration">Тривалість відображення.</param>
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

    private Coroutine currentDeathScreenAnimation;

    /// <summary>
    /// Показує екран смерті з анімацією появи.
    /// </summary>
    public void ShowDeathScreen()
    {
        if (currentDeathScreenAnimation != null)
            StopCoroutine(currentDeathScreenAnimation);

        bossUIPanel?.SetActive(false);
        deathScreenPanel.SetActive(true);
        deathScreenCanvasGroup.alpha = 0f;

        currentDeathScreenAnimation = StartCoroutine(AnimateDeathScreen(1f));
    }

    /// <summary>
    /// Ховає екран смерті з анімацією зникнення.
    /// </summary>
    public void HideDeathScreen()
    {
        if (currentDeathScreenAnimation != null)
            StopCoroutine(currentDeathScreenAnimation);

        currentDeathScreenAnimation = StartCoroutine(AnimateDeathScreen(0f));
    }

    private IEnumerator AnimateDeathScreen(float targetAlpha)
    {
        if (deathScreenCanvasGroup == null)
        {
            Debug.LogError("Death Screen CanvasGroup is not assigned!");
            yield break;
        }

        float currentAlpha = deathScreenCanvasGroup.alpha;
        
        while (!Mathf.Approximately(currentAlpha, targetAlpha))
        {
            currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime);
            deathScreenCanvasGroup.alpha = currentAlpha;
            yield return null;
        }

        if (Mathf.Approximately(targetAlpha, 0f))
        {
            deathScreenPanel.SetActive(false);
        }

        currentDeathScreenAnimation = null;
    }

    /// <summary>
    /// Ініціалізує UI менеджер.
    /// </summary>
    private void Start()
    {
        if (deathScreenPanel != null)
            deathScreenPanel.SetActive(false);

        if (retryButton != null)
            retryButton.onClick.AddListener(OnRetryButtonClick);
        
        if (menuButton != null)
            menuButton.onClick.AddListener(OnMenuButtonClick);
    }

    private void OnRetryButtonClick()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    private void OnMenuButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Очищує слухачів кнопок при знищенні об'єкта.
    /// </summary>
    private void OnDestroy()
    {
        if (retryButton != null)
            retryButton.onClick.RemoveListener(OnRetryButtonClick);
        
        if (menuButton != null)
            menuButton.onClick.RemoveListener(OnMenuButtonClick);
    }
}
