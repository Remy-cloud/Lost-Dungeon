using UnityEngine;

public class StaircaseInteractable : MonoBehaviour, IInteractable
{
    [Header("State")]
    [SerializeField] private bool unlocked = false;

    [Header("Physical Blocker")]
    [SerializeField] private GameObject physicalBlocker; // invisible wall blocking access until unlocked

    [Header("Transition Target")]
    [SerializeField] private Transform nextLevelSpawnPoint;
    [SerializeField] private int levelJustCompleted = 2;

    public bool CanInteract => unlocked;

    void Start()
    {
        if (physicalBlocker != null)
            physicalBlocker.SetActive(!unlocked); // active (blocking) only while locked
    }

    public void Unlock()
    {
        unlocked = true;
        Debug.Log("[Staircase] Unlocked! Player can now climb.");

        if (physicalBlocker != null)
            physicalBlocker.SetActive(false); // remove the block
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

