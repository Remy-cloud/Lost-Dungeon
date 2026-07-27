using UnityEngine;

public class TowerTeleportTrigger : MonoBehaviour
{
    [SerializeField] private Transform teleportDestination;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;
        other.transform.position = teleportDestination.position;
        if (cc != null) cc.enabled = true;

        Debug.Log("[TowerTeleport] Player teleported to tower fight area");
    }
}
