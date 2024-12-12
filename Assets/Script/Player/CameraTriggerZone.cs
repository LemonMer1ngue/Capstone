using UnityEngine;

public class CameraTriggerZone : MonoBehaviour
{
    [Header("Camera Settings when Triggered")]
    public Vector3 newOffset = new Vector3(0, 5, -10); // New offset when entering the trigger zone (in 2D, z value stays the same)
    public float newFoV = 15f; // New Field of View when entering the trigger zone
    public Vector2 newMinBounds = new Vector2(-5, -3); // New minimum bounds when entering the trigger zone
    public Vector2 newMaxBounds = new Vector2(5, 3); // New maximum bounds when entering the trigger zone

    private Camera cam;
    private EnhancedCameraFollow cameraFollow;

    private Vector3 originalOffset;
    private float originalFoV;
    private Vector2 originalMinBounds;
    private Vector2 originalMaxBounds;

    private void Start()
    {
        // Get the camera and EnhancedCameraFollow component
        cam = Camera.main;
        cameraFollow = cam.GetComponent<EnhancedCameraFollow>();

        if (cameraFollow == null)
        {
            Debug.LogError("EnhancedCameraFollow script not found on the main camera!");
        }

        // Store the original values of FoV, offset, and bounds
        originalOffset = cameraFollow.offset;
        originalFoV = cameraFollow.normalFoV;
        originalMinBounds = cameraFollow.minBounds;
        originalMaxBounds = cameraFollow.maxBounds;
    }

    // OnTriggerEnter2D for 2D trigger detection
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Change camera settings in EnhancedCameraFollow script when the player enters the trigger zone
            if (cameraFollow != null)
            {
                cameraFollow.offset = newOffset;
                cameraFollow.minBounds = newMinBounds;
                cameraFollow.maxBounds = newMaxBounds;
                cameraFollow.normalFoV = newFoV; // Set the new field of view directly here
                cam.fieldOfView = newFoV; // Set the field of view immediately without smooth transition
            }
        }
    }

    // OnTriggerExit2D for 2D trigger detection
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Reset camera settings in EnhancedCameraFollow script when the player exits the trigger zone
            if (cameraFollow != null)
            {
                cameraFollow.offset = originalOffset;
                cameraFollow.minBounds = originalMinBounds;
                cameraFollow.maxBounds = originalMaxBounds;
                cameraFollow.normalFoV = originalFoV; // Reset the field of view here
                cam.fieldOfView = originalFoV; // Set the field of view immediately without smooth transition
            }
        }
    }
}
