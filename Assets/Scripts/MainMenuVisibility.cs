using UnityEngine;

public class MainMenuVisibility : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCanvas;

    void Start()
    {
        ShowMenu();
    }

    public void ShowMenu()
    {
        mainMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void HideMenu()
    {
        mainMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
    }
}
