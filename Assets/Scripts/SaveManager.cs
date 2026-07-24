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
        Debug.Log($"Game saved to {SavePath}");
    }

    public void Load()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            CurrentData = JsonUtility.FromJson<PlayerSaveData>(json);
        }
        else
        {
            CurrentData = new PlayerSaveData(); // fresh save
        }
    }

    public void CompleteLevel(int levelIndex)
    {
        if (!CurrentData.completedLevels.Contains(levelIndex))
            CurrentData.completedLevels.Add(levelIndex);

        int nextLevel = levelIndex + 1;
        if (nextLevel > CurrentData.highestUnlockedLevel)
        {
            CurrentData.highestUnlockedLevel = nextLevel;
            OnLevelUnlocked?.Invoke(nextLevel);
        }

        OnLevelCompleted?.Invoke(levelIndex);
        Save();
    }

    public bool IsLevelUnlocked(int levelIndex) => levelIndex <= CurrentData.highestUnlockedLevel;
    public bool IsLevelCompleted(int levelIndex) => CurrentData.completedLevels.Contains(levelIndex);
}
