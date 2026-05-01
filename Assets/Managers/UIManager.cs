using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button restartButton;
    public TextMeshProUGUI maxScore;
    public TextMeshProUGUI roundCounter;
    public TextMeshProUGUI enemyKillCount;
    int actualKillCount = 0;
    int maxScorePoints;

    public static event Action OnStartButtonPressed;
    public static event Action OnGameRestart;

    void Awake()
    {
        restartButton.gameObject.SetActive(false);
        EnemyBehavior.OnEnemyKilled += AddKillCounter;
        GameManager.OnGameOver += ShowRestartButton;
        SpawnManager.OnRoundStart += AddToRound;
    }
    private void OnDestroy()
    {
        EnemyBehavior.OnEnemyKilled -= AddKillCounter;
        GameManager.OnGameOver -= ShowRestartButton;
        SpawnManager.OnRoundStart -= AddToRound;
    }
    public void StartGame()
    {
        OnStartButtonPressed?.Invoke();      
        startButton.gameObject.SetActive(false);
        roundCounter.text = "Round: " + 1;
        enemyKillCount.text = "Count: 0";
        actualKillCount = 0;
    }
    void AddToRound(int actualRound)
    {
        roundCounter.text = "Round: " + actualRound;
    }
    public void AddKillCounter()
    {
        actualKillCount++;
        enemyKillCount.text = "Count: " + actualKillCount.ToString();
        if (actualKillCount > maxScorePoints)
        {
            maxScorePoints = actualKillCount;
            maxScore.text = "Max Score: " + maxScorePoints.ToString();
        }
    }
    void ShowRestartButton(GameState currentState)
    {
        if(currentState == GameState.GameOver) restartButton.gameObject.SetActive(true);
    }
    public void RestartGame()
    {
        OnGameRestart?.Invoke();
        startButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(false);
        roundCounter.text = "Round: 0";
        enemyKillCount.text = "Count: 0";
    }
}
