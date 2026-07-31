using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel; // your original MenuPanel
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private LevelSelectUI levelSelectUI;
    [SerializeField] private GameObject settingsPanel;

    public void OnStartGame()
    {
        Debug.Log("[MainMenu] Start Game pressed");
        levelSelectUI.OnLevelButtonClicked(1); // always starts at Level 1
    }

    public void OnContinue()
    {
        int lastLevel = SaveManager.Instance.CurrentData.highestUnlockedLevel;
        Debug.Log($"[MainMenu] Continue pressed — Level {lastLevel}");
        levelSelectUI.OnLevelButtonClicked(lastLevel);
    }

    public void OnLevelSelection()
    {
        Debug.Log("[MainMenu] Level Selection pressed");
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
    }

    public void OnSettings()
    {
        Debug.Log("[MainMenu] Settings pressed");
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnExit()
    {
        Debug.Log("[MainMenu] Exit pressed");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
