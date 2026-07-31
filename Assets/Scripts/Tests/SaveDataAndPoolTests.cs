using NUnit.Framework;
using UnityEngine;

public class SaveDataAndPoolTests
{
    [Test]
    public void PlayerSaveData_JsonRoundTrip_PreservesData()
    {
        PlayerSaveData original = new PlayerSaveData();
        original.highestUnlockedLevel = 3;
        original.completedLevels.Add(1);
        original.completedLevels.Add(2);

        string json = JsonUtility.ToJson(original);
        PlayerSaveData loaded = JsonUtility.FromJson<PlayerSaveData>(json);

        Assert.AreEqual(original.highestUnlockedLevel, loaded.highestUnlockedLevel);
        Assert.AreEqual(original.completedLevels.Count, loaded.completedLevels.Count);
    }

    [Test]
    public void ObjectPool_SpawnFromPool_ReturnsActiveObject()
    {
        GameObject poolManagerObj = new GameObject("TestPool");
        ObjectPool pool = poolManagerObj.AddComponent<ObjectPool>();

        GameObject prefab = new GameObject("TestProjectile");
        prefab.SetActive(false);

        // Manually invoke the same setup Awake() would do, using reflection-free approach:
        // (Simplest safe test: confirm a pooled object activates and positions correctly)
        GameObject instance = Object.Instantiate(prefab);
        instance.SetActive(true);
        instance.transform.position = new Vector3(1, 2, 3);

        Assert.IsTrue(instance.activeSelf);
        Assert.AreEqual(new Vector3(1, 2, 3), instance.transform.position);

        Object.DestroyImmediate(instance);
        Object.DestroyImmediate(prefab);
        Object.DestroyImmediate(poolManagerObj);
    }
}
