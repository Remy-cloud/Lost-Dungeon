using UnityEngine;

public class PauseMenuTrigger : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCanvas;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mainMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
