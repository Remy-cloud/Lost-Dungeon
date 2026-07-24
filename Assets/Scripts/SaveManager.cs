using System;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    public PlayerSaveData CurrentData { get; private set; }

    // Events — Observer pattern, so UI/other systems react without direct coupling
    public static event Action<int> OnLevelCompleted;   // levelIndex just completed
    public static event Action<int> OnLevelUnlocked;    // levelIndex newly unlocked

    private string SavePath => Path.Combine(Application.persistentDataPath, "savegame.json");

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Load();
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(CurrentData, true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"[SaveManager] Game saved → {SavePath}");
    }

    public void Load()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            CurrentData = JsonUtility.FromJson<PlayerSaveData>(json);
            Debug.Log("[SaveManager] Save file loaded successfully");
        }
        else
        {
            CurrentData = new PlayerSaveData(); // fresh save
            Debug.Log("[SaveManager] No save file found — created fresh data");
        }
    }

    public void CompleteLevel(int levelIndex)
    {
        if (!CurrentData.completedLevels.Contains(levelIndex))
            CurrentData.completedLevels.Add(levelIndex);

        Debug.Log($"[SaveManager] Level {levelIndex} marked complete");

        int nextLevel = levelIndex + 1;
        if (nextLevel > CurrentData.highestUnlockedLevel)
        {
            CurrentData.highestUnlockedLevel = nextLevel;
            Debug.Log($"[SaveManager] Level {nextLevel} unlocked!");
            OnLevelUnlocked?.Invoke(nextLevel);
        }

        OnLevelCompleted?.Invoke(levelIndex);
        Save();
    }

    public bool IsLevelUnlocked(int levelIndex) => levelIndex <= CurrentData.highestUnlockedLevel;
    public bool IsLevelCompleted(int levelIndex) => CurrentData.completedLevels.Contains(levelIndex);
}
