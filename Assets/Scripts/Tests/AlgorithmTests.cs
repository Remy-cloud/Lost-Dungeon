using System.Collections.Generic;
using NUnit.Framework;

public class AlgorithmTests
{
    [Test]
    public void CompletedLevels_SortInAscendingOrder()
    {
        List<int> completedLevels = new List<int> { 3, 1, 4, 2 };

        completedLevels.Sort();

        CollectionAssert.AreEqual(new List<int> { 1, 2, 3, 4 }, completedLevels);
    }

    [Test]
    public void CriticalHitFormula_DoublesDamage_WhenTriggered()
    {
        float baseDamage = 20f;
        float critMultiplier = 2f;
        bool isCriticalHit = true;

        float finalDamage = isCriticalHit ? baseDamage * critMultiplier : baseDamage;

        Assert.AreEqual(40f, finalDamage);
    }

    [Test]
    public void LevelUnlockRule_UnlocksNextLevel_OnlyWhenHigherThanCurrent()
    {
        int highestUnlocked = 2;
        int levelJustCompleted = 2;
        int nextLevel = levelJustCompleted + 1;

        if (nextLevel > highestUnlocked)
        {
            highestUnlocked = nextLevel;
        }

        Assert.AreEqual(3, highestUnlocked);
    }
}
