using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System.Collections;

public class InteractableCameraEffect : MonoBehaviour
{
    [Header("Interactable Settings")]
    public GameObject[] interactableAreas; // Array of interactable GameObjects
    public GameObject player; // Player character GameObject
    public GameObject[] platformsToToggle; // Platforms to toggle visibility
    public bool togglePlatforms = true; // Control platform toggling

    [Header("Effect Durations")]
    public float fadeDuration = 1f; // Duration for the fade effect
    public float shakeDuration = 0.2f; // Duration for camera shake
    public float effectDelay = 0.1f; // Delay before applying effects

    [Header("Camera Settings")]
    public Camera targetCamera; // Camera to apply effects to

    [Header("Zoom and Shake Parameters")]
    public float zoomAmount = 1f; // Amount to zoom in
    public float zoomInDuration = 0.5f; // Duration of zoom-in effect
    public float zoomOutDuration = 0.2f; // Duration of zoom-out effect
    public float shakeIntensity = 1f; // Maximum intensity of shake

    [Header("Post-Processing Effects")]
    public bool enableSaturation = true; // Toggle saturation effect
    public float targetSaturation = 0f; // Target saturation level (-100 for grayscale)
    public bool enableContrast = true; // Toggle contrast effect
    public float targetContrast = 10f; // Target contrast value
    public bool enableChromaticAberration = true; // Toggle chromatic aberration
    public float targetCAIntensity = 0.5f; // Chromatic aberration intensity (0-1)

    [Header("Additional Post-Processing Effects")]
    public bool enableVignette = true; // Toggle vignette effect
    public float targetVignetteIntensity = 0.5f; // Vignette intensity
    public bool enableGrain = true; // Toggle grain effect
    public float targetGrainIntensity = 0.5f; // Grain intensity

    private PostProcessVolume volume;
    private ColorGrading colorGrading;
    private ChromaticAberration CA;
    private Vignette vignette;
    private Grain grain;

    private Coroutine fadeCoroutine;
    private float lastInteractTime = 0f; // Track last interaction time
    public float interactionCooldown = 1f; // Cooldown duration in seconds

    private void Start()
    {
        targetCamera ??= Camera.main; // Fallback to main camera if none specified
        volume = targetCamera.GetComponent<PostProcessVolume>();
        colorGrading = volume.profile.GetSetting<ColorGrading>();
        CA = volume.profile.GetSetting<ChromaticAberration>();
        vignette = volume.profile.GetSetting<Vignette>();
        grain = volume.profile.GetSetting<Grain>();
    }

    private void Update()
    {
        if (IsPlayerInAnyArea() && Input.GetKeyDown(KeyCode.E))
        {
            // Check if the cooldown has elapsed
            if (Time.time >= lastInteractTime + interactionCooldown)
            {
                lastInteractTime = Time.time; // Update the last interaction time
                if (fadeCoroutine != null)
                {
                    StopCoroutine(fadeCoroutine);
                }
                fadeCoroutine = StartCoroutine(FadeEffects());
            }
        }
    }

    private bool IsPlayerInAnyArea()
    {
        foreach (GameObject area in interactableAreas)
        {
            if (area.TryGetComponent<Collider2D>(out var areaCollider) && areaCollider.OverlapPoint(player.transform.position))
            {
                return true; // Player is in at least one area
            }
        }
        return false; // Player is not in any area
    }

    private IEnumerator FadeEffects()
    {
        // Start shaking and zooming
        StartCoroutine(ShakeAndZoom(shakeDuration));

        // Store starting values
        var startingValues = new
        {
            saturation = colorGrading.saturation.value,
            contrast = colorGrading.contrast.value,
            ca = CA.intensity.value,
            vignette = vignette.intensity.value,
            grain = grain.intensity.value
        };

        // Toggle effect states and determine target values
        bool[] toggles =
        {
            enableSaturation,
            enableContrast,
            enableChromaticAberration,
            enableVignette,
            enableGrain
        };

        float[] targetValues =
        {
            toggles[0] ? (colorGrading.saturation.value == targetSaturation ? 0f : targetSaturation) : 0f,
            toggles[1] ? (colorGrading.contrast.value == targetContrast ? 0f : targetContrast) : 0f,
            toggles[2] ? (CA.intensity.value == targetCAIntensity ? 0f : targetCAIntensity) : 0f,
            toggles[3] ? (vignette.intensity.value == targetVignetteIntensity ? 0f : targetVignetteIntensity) : 0f,
            toggles[4] ? (grain.intensity.value == targetGrainIntensity ? 0f : targetGrainIntensity) : 0f
        };

        // Wait for the effect delay before starting to fade
        yield return new WaitForSeconds(effectDelay);

        float elapsedTime = 0f;

        // Fade in/out for each effect
        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;

            // Lerp values based on toggled options
            if (enableSaturation) colorGrading.saturation.value = Mathf.Lerp(startingValues.saturation, targetValues[0], t);
            if (enableContrast) colorGrading.contrast.value = Mathf.Lerp(startingValues.contrast, targetValues[1], t);
            if (enableChromaticAberration) CA.intensity.value = Mathf.Lerp(startingValues.ca, targetValues[2], t);
            if (enableVignette) vignette.intensity.value = Mathf.Lerp(startingValues.vignette, targetValues[3], t);
            if (enableGrain) grain.intensity.value = Mathf.Lerp(startingValues.grain, targetValues[4], t);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure final values are set after the fade
        colorGrading.saturation.value = targetValues[0];
        colorGrading.contrast.value = targetValues[1];
        CA.intensity.value = targetValues[2];
        vignette.intensity.value = targetValues[3];
        grain.intensity.value = targetValues[4];

        // Toggle platform visibility if enabled
        if (togglePlatforms)
        {
            TogglePlatforms();
        }
    }

    private void TogglePlatforms()
    {
        foreach (GameObject platform in platformsToToggle)
        {
            platform.SetActive(!platform.activeSelf); // Toggle visibility
        }
    }

    private IEnumerator ShakeAndZoom(float duration)
    {
        Vector3 originalPosition = transform.localPosition; // Store the original position
        float originalSize = targetCamera.orthographicSize; // Store the original orthographic size

        float elapsed = 0f;

        // Zoom in and shake simultaneously
        while (elapsed < duration)
        {
            float magnitude = Mathf.Lerp(0, shakeIntensity, elapsed / duration);
            transform.localPosition = originalPosition + new Vector3(Random.Range(-1f, 1f) * magnitude, Random.Range(-1f, 1f) * magnitude, 0);

            targetCamera.orthographicSize = Mathf.Lerp(originalSize, originalSize - zoomAmount, elapsed / zoomInDuration);
            elapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Reset position and zoom out quickly
        transform.localPosition = originalPosition;
        elapsed = 0f;
        while (elapsed < zoomOutDuration)
        {
            targetCamera.orthographicSize = Mathf.Lerp(originalSize - zoomAmount, originalSize, elapsed / zoomOutDuration);
            elapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        targetCamera.orthographicSize = originalSize; // Ensure we set back to the original size
    }
}
