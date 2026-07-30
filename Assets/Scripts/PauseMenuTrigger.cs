using UnityEngine;

public class PauseMenuTrigger : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject levelSelectPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mainMenuCanvas.SetActive(true);
            mainMenuPanel.SetActive(true);
            levelSelectPanel.SetActive(false);
            Time.timeScale = 0f;
        }
    }
}
