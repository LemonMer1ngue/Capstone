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

    [Header("Zoom Settings")]
    public float normalFoV = 10f; // Default field of view
    public float zoomFoV = 17f; // Field of view when Tab is pressed
    public float zoomSpeed = 5f; // Speed of zoom transition

    private Camera cam;
    private bool isZoomed = false; // State of zoom

    private void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            target =  player.transform;
            if (target == null)
            {
                Debug.LogError("target tidak ditemukan pada object player!");
            }
        }
        else
        {
            Debug.LogError("Player tidak ditemukan di scene!");
        }
    
    }
    private void Start()
    {
        cam = Camera.main; // Get the main camera
        if (cam == null)
        {
            Debug.LogError("Main camera not found!");
        }
        else
        {
            cam.fieldOfView = normalFoV; // Set initial field of view
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

        HandleZoom(); // Handle zooming when Tab is pressed
    }

    private void HandleZoom()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isZoomed = !isZoomed; // Toggle zoom state
        }

        float targetFoV = isZoomed ? zoomFoV : normalFoV;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFoV, Time.deltaTime * zoomSpeed);
    }
}
