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

    private Vector3 currentOffset;
    private float currentFoV;
    private Vector2 currentMinBounds;
    private Vector2 currentMaxBounds;

    private bool isTransitioning = false;

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

        currentOffset = originalOffset;
        currentFoV = originalFoV;
        currentMinBounds = originalMinBounds;
        currentMaxBounds = originalMaxBounds;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isTransitioning)
            {
                // Set the current state as the new starting point
                originalOffset = cameraFollow.offset;
                originalFoV = cameraFollow.normalFoV;
                originalMinBounds = cameraFollow.minBounds;
                originalMaxBounds = cameraFollow.maxBounds;
                transitionTime = 0f;
            }

            isTransitioning = true;
        }
    }

    private void Update()
    {
        if (isTransitioning)
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
        else
        {
            isTransitioning = false;
        }

        if (cameraFollow != null)
        {
            currentOffset = Vector3.Lerp(originalOffset, newOffset, lerpFactor);
            currentMinBounds = Vector2.Lerp(originalMinBounds, newMinBounds, lerpFactor);
            currentMaxBounds = Vector2.Lerp(originalMaxBounds, newMaxBounds, lerpFactor);
            currentFoV = Mathf.Lerp(originalFoV, newFoV, lerpFactor);

            cameraFollow.offset = currentOffset;
            cameraFollow.minBounds = currentMinBounds;
            cameraFollow.maxBounds = currentMaxBounds;
            cameraFollow.normalFoV = currentFoV;

            cam.fieldOfView = currentFoV;
        }
    }
}
