using UnityEngine;

public class EnhancedCameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // The target to follow
    public Vector3 offset = new Vector3(0, 2, -10); // Default offset

    [Header("Camera Movement Settings")]
    public float smoothSpeed = 0.125f; // How smoothly the camera follows
    public Vector2 minBounds = new Vector2(-10, -5); // Minimum boundaries for the camera
    public Vector2 maxBounds = new Vector2(10, 5); // Maximum boundaries for the camera


    private Camera cam;

    private void Start()
    {
        cam = Camera.main; // Get the main camera
        if (cam == null)
        {
            Debug.LogError("Main camera not found!");
        }
    }

    private void LateUpdate()
    {
        if (target == null) return; // Exit if no target is assigned

        // Calculate the target position
        Vector3 targetPosition = target.position + offset;

        // Clamp the target position to the defined boundaries
        targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);
        targetPosition.z = transform.position.z; // Keep the camera's z position constant

        // Smoothly interpolate between the camera's current position and the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

        // Handle zooming if enabled

    }
}
