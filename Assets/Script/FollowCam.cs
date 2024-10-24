using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;            // The target (player) to follow
    public Vector3 offset;              // Offset from the target
    public float smoothSpeed = 0.125f;  // Speed at which the camera follows the target

    public float minX, maxX;            // Boundaries for the camera's X position
    public float minY, maxY;            // Boundaries for the camera's Y position

    void LateUpdate()
    {
        if (target == null) return;

        // Calculate the desired position
        Vector3 desiredPosition = target.position + offset;

        // Clamp the desired position within the specified boundaries
        float clampedX = Mathf.Clamp(desiredPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(desiredPosition.y, minY, maxY);
        Vector3 clampedPosition = new Vector3(clampedX, clampedY, transform.position.z);

        // Smoothly interpolate to the clamped position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, clampedPosition, smoothSpeed);

        // Update the camera position
        transform.position = smoothedPosition;
    }
}
