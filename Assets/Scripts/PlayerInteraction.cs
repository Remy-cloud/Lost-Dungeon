using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private KeyCode interactKey = KeyCode.F;
    [SerializeField] private float interactRadius = 2.5f;

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        Collider[] nearby = Physics.OverlapSphere(transform.position, interactRadius);

        IInteractable closest = null;
        float closestDist = Mathf.Infinity;

        foreach (Collider col in nearby)
        {
            IInteractable interactable = col.GetComponent<IInteractable>();
            if (interactable == null || !interactable.CanInteract) continue;

            float dist = Vector3.Distance(transform.position, col.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = interactable;
            }
        }

        if (closest != null)
        {
            Debug.Log("[PlayerInteraction] Interacting with nearby object");
            closest.Interact(gameObject);
        }
        else
        {
            Debug.Log("[PlayerInteraction] Nothing to interact with nearby");
        }
    }
}
