using UnityEngine;
using UnityEngine.UI;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] private Button[] levelButtons; // assign Level1Button through Level5Button, in order
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform[] levelSpawnPoints; // matching spawn point per level, in order
    [SerializeField] private MainMenuVisibility menuVisibility;
    [SerializeField] private GameObject mainMenuPanel; // your original MenuPanel, to return to on Back

    void OnEnable()
    {
        RefreshLevelIcons();
    }

    private void RefreshLevelIcons()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i + 1; // levels are 1-based

            bool unlocked = SaveManager.Instance.IsLevelUnlocked(levelIndex);
            bool completed = SaveManager.Instance.IsLevelCompleted(levelIndex);

            Transform lockIcon = levelButtons[i].transform.Find("LockIcon");
            Transform completeIcon = levelButtons[i].transform.Find("CompleteIcon");

            if (lockIcon != null) lockIcon.gameObject.SetActive(!unlocked);
            if (completeIcon != null) completeIcon.gameObject.SetActive(completed);

            levelButtons[i].interactable = unlocked;
        }
    }

    public void OnLevelButtonClicked(int levelIndex)
    {
        if (!SaveManager.Instance.IsLevelUnlocked(levelIndex)) return;

        int safeIndex = Mathf.Clamp(levelIndex, 1, levelSpawnPoints.Length);

        Debug.Log($"[LevelSelect] Loading Level {safeIndex}");

        CharacterController cc = playerTransform.GetComponent<CharacterController>();
        try
        {
            if (cc != null) cc.enabled = false;
            playerTransform.position = levelSpawnPoints[safeIndex - 1].position;
        }
        finally
        {
            if (cc != null) cc.enabled = true;
        }

        GameManager.Instance.SetCurrentLevel(safeIndex);

        gameObject.SetActive(false); // hide Level Select panel
        menuVisibility.HideMenu(); // hide menu canvas entirely, unpause
    }

    public void OnBackPressed()
    {
        gameObject.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }
}
