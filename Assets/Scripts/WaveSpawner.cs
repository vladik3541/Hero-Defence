using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum WaveType
{
    WaitTime, SpawnTime
}
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
    private WaveType currentWaveType;
    
    public WaveType CurrentWaveType { get => currentWaveType; }
    public UnityEvent OnAllWavesCompleted;
    public event Action OnWaveStarted;
    public event Action OnWaveEnded;
    

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
        currentWaveType = WaveType.WaitTime;
        // Очікування перед першою хвилею
        yield return StartCoroutine(Countdown(startDelay));
        yield return StartCoroutine(StartWave(0));
    }

    private IEnumerator StartWave(int waveIndex)
    {
        currentWaveType = WaveType.SpawnTime;
        if (waveIndex >= wavesData.waves.Length)
        {
            OnAllWavesCompleted?.Invoke();
            timerText.text = "Win!";
            readyButton.gameObject.SetActive(false);
            yield break;
        }

        waveInProgress = true;
        currentWaveIndex = waveIndex;
        OnWaveStarted?.Invoke();
        timerText.text = $"Wave {currentWaveIndex + 1}";

        Wave wave = wavesData.waves[waveIndex];

        foreach (var enemyData in wave.enemies)
        {
            for (int i = 0; i < enemyData.count; i++)
            {
                GameObject enemy = Instantiate(enemyData.prefabEnemy, spawnPoint.position, Quaternion.identity);
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.bounty = enemyData.bounty;
                    enemyHealth.OnDead += OnEnemyDeath;
                }
                enemiesAlive++;
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        while (enemiesAlive > 0)
            yield return null;

        waveInProgress = false;
        OnWaveEnded?.Invoke();

        if (currentWaveIndex + 1 < wavesData.waves.Length)
        {
            currentWaveType = WaveType.WaitTime;
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
