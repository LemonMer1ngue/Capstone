using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class InteractableCameraEffect : MonoBehaviour
{
    [Header("Interactable Settings")]
    public GameObject[] interactableAreas; // Areas where interaction is possible
    public GameObject player; // Reference to the player object
    public GameObject[] platformsToToggle; // Platforms to toggle visibility
    public GameObject[] boxToToggle; 
    public bool togglePlatforms = true; // Whether to toggle platforms on interaction

    [Header("Sound Settings")]
    public AudioClip interactSound; // Sound played on interaction
    public AudioSource backgroundMusicSource; // Background music source
    public float changedPitch = 0.5f; // Pitch to change to
    public float pitchChangeTransitionDuration = 0.5f; // Duration for pitch change

    [Header("Effect Durations")]
    public float fadeDuration = 1f; // Duration for fading effects
    public float shakeDuration = 0.2f; // Duration for camera shake
    public float effectDelay = 0.1f; // Delay before effects start

    [Header("Camera Settings")]
    public Camera targetCamera; // Camera to apply effects to

    [Header("Zoom and Shake Parameters")]
    public float zoomAmount = 1f; // Amount to zoom in
    public float zoomInDuration = 0.5f; // Duration for zooming in
    public float zoomOutDuration = 0.2f; // Duration for zooming out
    public float shakeIntensity = 1f; // Intensity of shake effect

    [Header("Post-Processing Effects")]
    public bool enableSaturation = true; // Toggle for saturation effect
    public float targetSaturation = 0f; // Target saturation level
    public bool enableContrast = true; // Toggle for contrast effect
    public float targetContrast = 10f; // Target contrast level
    public bool enableChromaticAberration = true; // Toggle for chromatic aberration
    public float targetCAIntensity = 0.5f; // Target intensity for chromatic aberration

    [Header("Additional Post-Processing Effects")]
    public bool enableVignette = true; // Toggle for vignette effect
    public float targetVignetteIntensity = 0.5f; // Target vignette intensity
    public bool enableFilmGrain = true; // Toggle for film grain effect
    public float targetFilmGrainIntensity = 0.5f; // Target film grain intensity

    private Volume volume; // Volume component for post-processing
    private ColorAdjustments colorAdjustments; // Color adjustments settings
    private ChromaticAberration chromaticAberration; // Chromatic aberration settings
    private Vignette vignette; // Vignette settings
    private FilmGrain filmGrain; // Film grain settings

    private Coroutine fadeCoroutine; // Reference to the fade coroutine
    private float lastInteractTime = 0f; // Timestamp of the last interaction
    public float interactionCooldown = 1f; // Cooldown time between interactions

    private bool isPitchChanged = false; // Track if the pitch has been changed
    private bool effectsActive = false; // Track if effects are currently active

    [Header("Dunia nyata atau Dunia Gaib")]
    private bool isInSpiritWorld = false;



    private void Start()
    {
        // Initialize target camera and get post-processing components
        targetCamera ??= Camera.main;
        volume = targetCamera.GetComponent<Volume>();
        volume.profile.TryGet(out colorAdjustments);
        volume.profile.TryGet(out chromaticAberration);
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out filmGrain);
    }

    private void Update()
    {
        // Check if player is in any interactable area
        if (IsPlayerInAnyArea() && Input.GetKeyDown(KeyCode.E) && Time.time >= lastInteractTime + interactionCooldown)
        {
            lastInteractTime = Time.time; // Update last interaction time
            PlayInteractSound(); // Play interaction sound
            //StartCoroutine(ChangeBackgroundMusicPitch()); // Change background music pitch

            // Start fading effects
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(FadeEffects());
        }
    }

    private void PlayInteractSound()
    {
        // Play the interaction sound at the player's position
        if (interactSound != null)
        {
            AudioSource.PlayClipAtPoint(interactSound, player.transform.position);
        }
    }

    private IEnumerator ChangeBackgroundMusicPitch()
    {
        // Change the pitch of the background music over time
        float targetPitch = isPitchChanged ? 1f : changedPitch;
        float startPitch = backgroundMusicSource.pitch;
        float elapsed = 0f;

        while (elapsed < pitchChangeTransitionDuration)
        {
            backgroundMusicSource.pitch = Mathf.Lerp(startPitch, targetPitch, elapsed / pitchChangeTransitionDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        backgroundMusicSource.pitch = targetPitch; // Set final pitch
        isPitchChanged = !isPitchChanged; // Toggle pitch state
    }

    private bool IsPlayerInAnyArea()
    {
        // Check if the player is in any defined interactable area
        foreach (GameObject area in interactableAreas)
        {
            if (area.TryGetComponent<Collider2D>(out var areaCollider) && areaCollider.OverlapPoint(player.transform.position))
            {
                return true; // Player is within an interactable area
            }
        }
        return false; // Player is not in any area
    }

    private IEnumerator FadeEffects()
    {
        StartCoroutine(ShakeAndZoom(shakeDuration));

        // Get starting values for post-processing effects
        var startingValues = new
        {
            saturation = colorAdjustments.saturation.value,
            contrast = colorAdjustments.contrast.value,
            ca = chromaticAberration.intensity.value,
            vignette = vignette.intensity.value,
            filmGrain = filmGrain.intensity.value
        };

        // Determine target values based on effectsActive state
        float[] targetValues = effectsActive ? new float[] { 0f, 0f, 0f, 0f, 0f } : new float[]
        {
            enableSaturation ? targetSaturation : 0f,
            enableContrast ? targetContrast : 0f,
            enableChromaticAberration ? targetCAIntensity : 0f,
            enableVignette ? targetVignetteIntensity : 0f,
            enableFilmGrain ? targetFilmGrainIntensity : 0f
        };

        yield return new WaitForSeconds(effectDelay); // Delay before starting effects

        // Smoothly interpolate to target values over fade duration
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;

            if (enableSaturation) colorAdjustments.saturation.value = Mathf.Lerp(startingValues.saturation, targetValues[0], t);
            if (enableContrast) colorAdjustments.contrast.value = Mathf.Lerp(startingValues.contrast, targetValues[1], t);
            if (enableChromaticAberration) chromaticAberration.intensity.value = Mathf.Lerp(startingValues.ca, targetValues[2], t);
            if (enableVignette) vignette.intensity.value = Mathf.Lerp(startingValues.vignette, targetValues[3], t);
            if (enableFilmGrain) filmGrain.intensity.value = Mathf.Lerp(startingValues.filmGrain, targetValues[4], t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Set final values after interpolation
        colorAdjustments.saturation.value = targetValues[0];
        colorAdjustments.contrast.value = targetValues[1];
        chromaticAberration.intensity.value = targetValues[2];
        vignette.intensity.value = targetValues[3];
        filmGrain.intensity.value = targetValues[4];

        // Toggle platforms if enabled
        if (togglePlatforms)
        {
            TogglePlatforms();
        }

        ToggleBoxes();

        effectsActive = !effectsActive; // Toggle effects state

    }

    private void TogglePlatforms()
    {
        foreach (GameObject platform in platformsToToggle)
        {
            platform.SetActive(!platform.activeSelf);
        } 
    }


    private void ToggleBoxes()
    {
        switch (isInSpiritWorld)
        {
            case false:
                foreach (GameObject box in boxToToggle)
                {
                    box.SetActive(true);
                }
                break;

            case true:

                foreach (GameObject box in boxToToggle)
                {
                    box.SetActive(false);
                }
                break;
        }
    }


    private IEnumerator ShakeAndZoom(float duration)
    {
        // Store original camera position and size
        Vector3 originalPosition = transform.localPosition;
        float originalSize = targetCamera.orthographicSize;

        float elapsed = 0f;

        // Shake the camera while zooming in
        while (elapsed < duration)
        {
            float magnitude = Mathf.Lerp(0, shakeIntensity, elapsed / duration);
            transform.localPosition = originalPosition + new Vector3(Random.Range(-1f, 1f) * magnitude, Random.Range(-1f, 1f) * magnitude, 0);
            targetCamera.orthographicSize = Mathf.Lerp(originalSize, originalSize - zoomAmount, elapsed / zoomInDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset position and zoom out
        transform.localPosition = originalPosition;
        elapsed = 0f;

        while (elapsed < zoomOutDuration)
        {
            targetCamera.orthographicSize = Mathf.Lerp(originalSize - zoomAmount, originalSize, elapsed / zoomOutDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        targetCamera.orthographicSize = originalSize; // Ensure final size is set
    }
}
