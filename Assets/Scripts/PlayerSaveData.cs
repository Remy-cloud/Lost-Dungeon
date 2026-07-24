using System;
using System.Collections.Generic;

[Serializable]
public class PlayerSaveData
{
    // Level Progression
    public int highestUnlockedLevel = 1;
    public List<int> completedLevels = new List<int>();

    // Player Statistics
    public float maxHealth = 100f;
    public int enemiesDefeated = 0;
    public int deaths = 0;

    // Inventory
    public List<string> inventoryItemIds = new List<string>();

    // Settings
    public float musicVolume = 1f;
    public float sfxVolume = 1f;

    // Unlockable Content
    public List<string> unlockedAbilities = new List<string>();
}
