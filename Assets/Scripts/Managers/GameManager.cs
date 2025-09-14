// Scripts/Managers/GameManager.cs
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Керує основним ігровим циклом, включаючи запуск гри, появу боса та смерть гравця.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton екземпляр GameManager.
    /// </summary>
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private float preparationTime = 90f;
    [SerializeField] private float restartDelay = 4f;

    [Header("Boss Settings")]
    [SerializeField] private GameObject kolobokPrefab;
    [SerializeField] private Transform spawnPoint;

    /// <summary>
    /// Ініціалізує Singleton GameManager.
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    /// <summary>
    /// Запускає ігровий цикл.
    /// </summary>
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

    /// <summary>
    /// Обробляє смерть гравця, перезапускаючи рівень із затримкою.
    /// </summary>
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