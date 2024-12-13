using UnityEngine;

public class CameraTriggerZone : MonoBehaviour
{
    [Header("Camera Settings when Triggered")]
    public Vector3 newOffset = new Vector3(0, 5, -10);
    public float newFoV = 15f;
    public Vector2 newMinBounds = new Vector2(-5, -3);
    public Vector2 newMaxBounds = new Vector2(5, 3);

    [Header("Transition Settings")]
    public float transitionDuration = 2f;
    private float transitionTime = 0f;

    private Camera cam;
    private EnhancedCameraFollow cameraFollow;

    private Vector3 originalOffset;
    private float originalFoV;
    private Vector2 originalMinBounds;
    private Vector2 originalMaxBounds;

    private bool isPlayerInZone = false;
    private static CameraTriggerZone currentTriggerZone = null; // Static reference to the current active zone

    private void Start()
    {
        cam = Camera.main;
        cameraFollow = cam.GetComponent<EnhancedCameraFollow>();

        if (cameraFollow == null)
        {
            Debug.LogError("EnhancedCameraFollow script not found on the main camera!");
        }

        originalOffset = cameraFollow.offset;
        originalFoV = cameraFollow.normalFoV;
        originalMinBounds = cameraFollow.minBounds;
        originalMaxBounds = cameraFollow.maxBounds;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Ensure the previous trigger zone stops its transition
            if (currentTriggerZone != null && currentTriggerZone != this)
            {
                currentTriggerZone.ResetZone();
            }

            // Set this as the current active zone
            currentTriggerZone = this;

            isPlayerInZone = true;
            transitionTime = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Clear the current active zone if the player exits
            if (currentTriggerZone == this)
            {
                currentTriggerZone = null;
            }

            isPlayerInZone = false;
        }
    }

    private void Update()
    {
        if (isPlayerInZone)
        {
            SmoothTransitionToNewSettings();
        }
    }

    private void SmoothTransitionToNewSettings()
    {
        float lerpFactor = Mathf.Clamp01(transitionTime / transitionDuration);

        if (transitionTime < transitionDuration)
        {
            transitionTime += Time.deltaTime;
        }

        if (cameraFollow != null)
        {
            cameraFollow.offset = Vector3.Lerp(cameraFollow.offset, newOffset, lerpFactor);
            cameraFollow.minBounds = Vector2.Lerp(cameraFollow.minBounds, newMinBounds, lerpFactor);
            cameraFollow.maxBounds = Vector2.Lerp(cameraFollow.maxBounds, newMaxBounds, lerpFactor);
            cameraFollow.normalFoV = Mathf.Lerp(cameraFollow.normalFoV, newFoV, lerpFactor);

            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newFoV, lerpFactor);
        }
    }

    private void ResetZone()
    {
        isPlayerInZone = false;
        transitionTime = 0f;
    }
}
