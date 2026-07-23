using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target; // the player

    [Header("Third-Person Offset")]
    [SerializeField] private float distanceBehind = 5f;
    [SerializeField] private float height = 3f;

    [Header("Follow Smoothing")]
    [SerializeField] private float positionSmoothSpeed = 8f;
    [SerializeField] private float rotationSmoothSpeed = 8f;

    void LateUpdate()
    {
        if (target == null) return;

        // Position camera behind the player, based on player's current facing direction
        Vector3 desiredPosition = target.position
                                   - target.forward * distanceBehind
                                   + Vector3.up * height;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, positionSmoothSpeed * Time.deltaTime);

        // Look toward a point slightly above the player (so it's not staring at their feet)
        Vector3 lookTarget = target.position + Vector3.up * 1.5f;
        Quaternion desiredRotation = Quaternion.LookRotation(lookTarget - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmoothSpeed * Time.deltaTime);
    }
}
