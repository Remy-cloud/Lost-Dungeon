using UnityEngine;

public class StaircaseInteractable : MonoBehaviour, IInteractable
{
    [Header("State")]
    [SerializeField] private bool unlocked = false;

    [Header("Transition Target")]
    [SerializeField] private Transform nextLevelSpawnPoint;
    [SerializeField] private int levelJustCompleted = 2;

    public bool CanInteract => unlocked;

    public void Unlock()
    {
        unlocked = true;
        Debug.Log("[Staircase] Unlocked! Player can now climb.");
        // Optional: enable a visual cue here later (glow, particle, etc.)
    }

    public void Interact(GameObject interactor)
    {
        if (!unlocked)
        {
            Debug.Log("[Staircase] Locked — defeat the enemy first.");
            return;
        }

        Debug.Log("[Staircase] Climbing to next level...");

        CharacterController cc = interactor.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;
        interactor.transform.position = nextLevelSpawnPoint.position;
        if (cc != null) cc.enabled = true;

        SaveManager.Instance.CompleteLevel(levelJustCompleted);
        GameManager.Instance.SetCurrentLevel(levelJustCompleted + 1);
    }

}
