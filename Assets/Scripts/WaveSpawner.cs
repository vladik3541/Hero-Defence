using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private WavesData wavesData;
    [SerializeField] private Transform spawnPoint;

    [Header("Settings")]
    [SerializeField] private float startDelay = 60f;
    [SerializeField] private float betweenWavesDelay = 25f;
    [SerializeField] private float spawnInterval = 0.1f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Button readyButton;

    private int currentWaveIndex;
    private int enemiesAlive;
    private bool waveInProgress;
    private bool skipTimer;

    public UnityEvent OnAllWavesCompleted;
    public UnityEvent<int> OnWaveStarted;
    public UnityEvent<int> OnWaveEnded;

    private void Start()
    {
        readyButton.onClick.AddListener(SkipWaiting);
        StartCoroutine(WaveRoutine());
    }

    private void SkipWaiting()
    {
        skipTimer = true;
        readyButton.gameObject.SetActive(false);
    }

    private IEnumerator WaveRoutine()
    {
        // Очікування перед першою хвилею
        yield return StartCoroutine(Countdown(startDelay));
        yield return StartCoroutine(StartWave(0));
    }

    private IEnumerator StartWave(int waveIndex)
    {
        if (waveIndex >= wavesData.waves.Length)
        {
            OnAllWavesCompleted?.Invoke();
            timerText.text = "Win!";
            readyButton.gameObject.SetActive(false);
            yield break;
        }

        waveInProgress = true;
        currentWaveIndex = waveIndex;
        OnWaveStarted?.Invoke(currentWaveIndex + 1);
        timerText.text = $"Wave {currentWaveIndex + 1}";

        Wave wave = wavesData.waves[waveIndex];

        foreach (var enemyData in wave.enemies)
        {
            for (int i = 0; i < enemyData.count; i++)
            {
                GameObject enemy = Instantiate(enemyData.prefabEnemy, spawnPoint.position, Quaternion.identity);
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                    enemyHealth.OnDead += OnEnemyDeath;

                enemiesAlive++;
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        while (enemiesAlive > 0)
            yield return null;

        waveInProgress = false;
        OnWaveEnded?.Invoke(currentWaveIndex + 1);

        if (currentWaveIndex + 1 < wavesData.waves.Length)
        {
            yield return StartCoroutine(Countdown(betweenWavesDelay));
            yield return StartCoroutine(StartWave(currentWaveIndex + 1));
        }
        else
        {
            timerText.text = "All enemies won!";
            OnAllWavesCompleted?.Invoke();
        }
        currentWaveIndex++;
    }

    private void OnEnemyDeath()
    {
        enemiesAlive--;
    }

    private IEnumerator Countdown(float time)
    {
        skipTimer = false;

        float remaining = time;
        while (remaining > 0 && !skipTimer)
        {
            int minutes = Mathf.FloorToInt(remaining / 60);
            int seconds = Mathf.FloorToInt(remaining % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
            remaining -= Time.deltaTime;
            yield return null;
        }

        readyButton.gameObject.SetActive(false);
        timerText.text = "";
    }
}
