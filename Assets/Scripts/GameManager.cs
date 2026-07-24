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
        Debug.Log($"[GameManager] State changed → {newState}");
        OnGameStateChanged?.Invoke(newState);
    }

    public void SetCurrentLevel(int levelIndex)
    {
        CurrentLevelIndex = levelIndex;
        Debug.Log($"[GameManager] Current level set → {levelIndex}");
    }

    public void NotifyEnemyDefeated()
    {
        Debug.Log("[GameManager] Enemy defeated event fired");
        OnEnemyDefeated?.Invoke();
    }

    public void NotifyItemCollected(string itemId)
    {
        Debug.Log($"[GameManager] Item collected → {itemId}");
        OnItemCollected?.Invoke(itemId);
    }

    public void NotifyAbilityActivated(string abilityName)
    {
        Debug.Log($"[GameManager] Ability activated → {abilityName}");
        OnAbilityActivated?.Invoke(abilityName);
    }
}
