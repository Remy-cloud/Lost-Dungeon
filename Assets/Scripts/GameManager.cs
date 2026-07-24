using System;
using UnityEngine;

public enum GameState { MainMenu, Playing, Paused, GameOver }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; } = GameState.MainMenu;
    public int CurrentLevelIndex { get; private set; } = 1;

    public static event Action<GameState> OnGameStateChanged;
    public static event Action OnEnemyDefeated;
    public static event Action<string> OnItemCollected;
    public static event Action<string> OnAbilityActivated;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;
        OnGameStateChanged?.Invoke(newState);
    }

    public void SetCurrentLevel(int levelIndex)
    {
        CurrentLevelIndex = levelIndex;
    }

    public void NotifyEnemyDefeated() => OnEnemyDefeated?.Invoke();
    public void NotifyItemCollected(string itemId) => OnItemCollected?.Invoke(itemId);
    public void NotifyAbilityActivated(string abilityName) => OnAbilityActivated?.Invoke(abilityName);
}
