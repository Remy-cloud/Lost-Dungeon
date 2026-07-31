using UnityEngine;

public class CollectibleInteractable : MonoBehaviour, IInteractable
{
    [Header("Effect")]
    [SerializeField] private float healAmount = 100f;

    [Header("Level Progression")]
    [SerializeField] private int levelJustCompleted = 3;

    public bool CanInteract => true; // always interactable once revealed/active

    public void Interact(GameObject interactor)
    {
        Debug.Log("[COLLECTIBLE-HEALTHBOOST] Interact() triggered on the health orb");

        Health playerHealth = interactor.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.Heal(healAmount);
        }

        Debug.Log("[COLLECTIBLE-HEALTHBOOST] Health boost collected! Player healed.");

        GameManager.Instance?.NotifyItemCollected("HealthBoost");
        SaveManager.Instance.CompleteLevel(levelJustCompleted);

        gameObject.SetActive(false); // consumed, disappear
    }
}
