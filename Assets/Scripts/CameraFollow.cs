using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Third-Person Offset")]
    [SerializeField] private float distanceBehind = 5f;
    [SerializeField] private float height = 3f;

    [Header("Follow Smoothing")]
    [SerializeField] private float positionSmoothSpeed = 8f;
    [SerializeField] private float rotationSmoothSpeed = 8f;

    [Header("Collision Settings")]
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float collisionBuffer = 0.3f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 pivotPoint = target.position + Vector3.up * height;
        Vector3 desiredCameraPos = pivotPoint - target.forward * distanceBehind;

        // Check if anything blocks the line from pivot to desired camera position
        float finalDistance = distanceBehind;
        if (Physics.Raycast(pivotPoint, -target.forward, out RaycastHit hit, distanceBehind, collisionMask))
        {
            finalDistance = hit.distance - collisionBuffer;
        }

        Vector3 finalPosition = pivotPoint - target.forward * finalDistance;

        transform.position = Vector3.Lerp(transform.position, finalPosition, positionSmoothSpeed * Time.deltaTime);

        Quaternion desiredRotation = Quaternion.LookRotation(pivotPoint - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmoothSpeed * Time.deltaTime);
    }
}
