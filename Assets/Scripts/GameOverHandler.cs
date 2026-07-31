using UnityEngine;
using TMPro;

public class GameOverHandler : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject gameStatePanel;
    [SerializeField] private TextMeshProUGUI gameStateMessage;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject mainMenuPanel;

    void OnEnable()
    {
        Health.OnDeath += HandleDeath;
        SaveManager.OnLevelCompleted += HandleLevelCompleted;
    }

    void OnDisable()
    {
        Health.OnDeath -= HandleDeath;
        SaveManager.OnLevelCompleted -= HandleLevelCompleted;
    }

    private void HandleDeath(Health health)
    {
        if (health.gameObject != playerTransform.gameObject) return;

        DisablePlayerControl();
        ShowPanel("GAME OVER");
    }

    private void HandleLevelCompleted(int levelIndex)
    {
        if (levelIndex != 5) return;

        ShowPanel("VICTORY! Dungeon Cleared");
    }

    private void ShowPanel(string message)
    {
        gameStateMessage.text = message;
        mainMenuCanvas.SetActive(true);
        mainMenuPanel.SetActive(false);
        gameStatePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void DisablePlayerControl()
    {
        playerTransform.GetComponent<PlayerController>().enabled = false;
        playerTransform.GetComponent<PlayerCombat>().enabled = false;
        playerTransform.GetComponent<AbilityController>().enabled = false;
    }

    public void RevivePlayer()
    {
        playerTransform.GetComponent<Health>().Revive();
        playerTransform.GetComponent<PlayerAnimationJuice>().ResetAfterRevive();
        playerTransform.GetComponent<PlayerController>().enabled = true;
        playerTransform.GetComponent<PlayerCombat>().enabled = true;
        playerTransform.GetComponent<AbilityController>().enabled = true;
    }
}
