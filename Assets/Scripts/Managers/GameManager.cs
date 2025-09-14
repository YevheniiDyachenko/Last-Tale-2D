// Scripts/Managers/GameManager.cs
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Для роботи зі сценами

public class GameManager : MonoBehaviour
{
    // --- Створення Singleton ---
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private float preparationTime = 90f;
    [SerializeField] private float restartDelay = 4f; // Затримка перед перезапуском

    [Header("Boss Settings")]
    [SerializeField] private GameObject kolobokPrefab;
    [SerializeField] private Transform spawnPoint;

    private void Awake()
    {
        // Налаштування Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        Debug.Log("Фаза підготовки почалася!");
        yield return new WaitForSeconds(preparationTime);
        SpawnBoss();
    }

    private void SpawnBoss()
    {
        if (kolobokPrefab != null)
        {
            Instantiate(kolobokPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    // --- НОВИЙ МЕТОД для перезапуску ---
    public void PlayerDied()
    {
        Debug.Log("GameManager received PlayerDied signal. Restarting level...");
        StartCoroutine(RestartLevelRoutine());
    }

    private IEnumerator RestartLevelRoutine()
    {
        yield return new WaitForSeconds(restartDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}