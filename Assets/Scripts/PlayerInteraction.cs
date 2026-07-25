using UnityEngine;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private KeyCode interactKey = KeyCode.F;

    private List<IInteractable> nearbyInteractables = new List<IInteractable>();

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        foreach (var interactable in nearbyInteractables)
        {
            if (interactable != null && interactable.CanInteract)
            {
                Debug.Log("[PlayerInteraction] Interacting with nearby object");
                interactable.Interact(gameObject);
                return; // only interact with the first valid one found
            }
        }

        Debug.Log("[PlayerInteraction] Nothing to interact with nearby");
    }

    void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null && !nearbyInteractables.Contains(interactable))
        {
            nearbyInteractables.Add(interactable);
            Debug.Log($"[PlayerInteraction] In range of: {other.gameObject.name}");
        }
    }

    void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            nearbyInteractables.Remove(interactable);
        }
    }
}
