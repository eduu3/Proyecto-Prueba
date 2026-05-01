using UnityEngine;
using System;

public enum GameState
{
    WaitingToStart,
    Playing,
    GameOver
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState currentState = GameState.WaitingToStart;
    public static event Action <GameState> OnGameOver;
    public static event Action <GameState> OnStart;
    public static event Action <GameState> OnRestartGame;

    private void Awake()
    {
        Instance = this;
        UIManager.OnStartButtonPressed += StartGame;
        UIManager.OnGameRestart += RestartGame;
        PlayerStats.OnPlayerDeath += GameOver;
        
    }
    private void OnDestroy()
    {
        UIManager.OnStartButtonPressed -= StartGame;
        UIManager.OnGameRestart -= RestartGame;
        PlayerStats.OnPlayerDeath -= GameOver;

    }
    public void StartGame()
    {
        currentState = GameState.Playing;
        OnStart?.Invoke(currentState);
    }
    void GameOver()
    {
        currentState = GameState.GameOver;
        OnGameOver?.Invoke(currentState);
    }
    void RestartGame()
    {
        currentState = GameState.WaitingToStart;
        OnRestartGame?.Invoke(currentState);
    }
}
