using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void OnStartGame()
    {
        Debug.Log("[MainMenu] Start Game pressed");
        // TODO: load gameplay scene from Level 1
    }

    public void OnContinue()
    {
        int lastLevel = SaveManager.Instance.CurrentData.highestUnlockedLevel;
        Debug.Log($"[MainMenu] Continue pressed — Level {lastLevel}");
        // TODO: load gameplay scene at last unlocked level
    }

    public void OnLevelSelection()
    {
        Debug.Log("[MainMenu] Level Selection pressed");
        // TODO: show Level Select panel
    }

    public void OnSettings()
    {
        Debug.Log("[MainMenu] Settings pressed");
        // TODO: show Settings panel
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
