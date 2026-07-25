using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private int levelJustCompleted = 1;
    [SerializeField] private Transform nextLevelSpawnPoint;
    [SerializeField] private Transform playerTransform;

    public void TriggerTransition()
    {
        ScreenFader.Instance.FadeOutThenIn(() =>
        {
            // Teleport happens here, while the screen is black
            CharacterController cc = playerTransform.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false; // must disable before moving a CharacterController
            playerTransform.position = nextLevelSpawnPoint.position;
            if (cc != null) cc.enabled = true;

            SaveManager.Instance.CompleteLevel(levelJustCompleted);
            GameManager.Instance.SetCurrentLevel(levelJustCompleted + 1);
        });
    }
}
