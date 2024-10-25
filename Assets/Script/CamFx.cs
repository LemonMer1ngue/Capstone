using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class InteractableCameraEffect : MonoBehaviour
{
    [Header("Interactable Settings")]
    public GameObject[] interactableAreas;
    public GameObject player;
    public GameObject[] platformsToToggle;
    public bool togglePlatforms = true;

    [Header("Sound Settings")]
    public AudioClip interactSound;
    public AudioSource backgroundMusicSource;
    public float changedPitch = 0.5f;
    public float pitchChangeTransitionDuration = 0.5f;

    [Header("Effect Durations")]
    public float fadeDuration = 1f;
    public float shakeDuration = 0.2f;
    public float effectDelay = 0.1f;

    [Header("Camera Settings")]
    public Camera targetCamera;

    [Header("Zoom and Shake Parameters")]
    public float zoomAmount = 1f;
    public float zoomInDuration = 0.5f;
    public float zoomOutDuration = 0.2f;
    public float shakeIntensity = 1f;

    [Header("Post-Processing Effects")]
    public bool enableSaturation = true;
    public float targetSaturation = 0f;
    public bool enableContrast = true;
    public float targetContrast = 10f;
    public bool enableChromaticAberration = true;
    public float targetCAIntensity = 0.5f;

    [Header("Additional Post-Processing Effects")]
    public bool enableVignette = true;
    public float targetVignetteIntensity = 0.5f;
    public bool enableFilmGrain = true;
    public float targetFilmGrainIntensity = 0.5f;

    private Volume volume;
    private ColorAdjustments colorAdjustments;
    private ChromaticAberration chromaticAberration;
    private Vignette vignette;
    private FilmGrain filmGrain;

    private Coroutine fadeCoroutine;
    private float lastInteractTime = 0f;
    public float interactionCooldown = 1f;

    private bool isPitchChanged = false;

    private void Start()
    {
        targetCamera ??= Camera.main;
        volume = targetCamera.GetComponent<Volume>();
        volume.profile.TryGet(out colorAdjustments);
        volume.profile.TryGet(out chromaticAberration);
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out filmGrain);
    }

    private void Update()
    {
        if (IsPlayerInAnyArea())
        {
            if (Input.GetKeyDown(KeyCode.E) && Time.time >= lastInteractTime + interactionCooldown)
            {
                lastInteractTime = Time.time;

                PlayInteractSound();
                StartCoroutine(ChangeBackgroundMusicPitch());

                if (fadeCoroutine != null)
                {
                    StopCoroutine(fadeCoroutine);
                }
                fadeCoroutine = StartCoroutine(FadeEffects());
            }
        }
    }

    private void PlayInteractSound()
    {
        if (interactSound != null)
        {
            AudioSource.PlayClipAtPoint(interactSound, player.transform.position);
        }
    }

    private IEnumerator ChangeBackgroundMusicPitch()
    {
        float targetPitch = isPitchChanged ? 1f : changedPitch;
        float startPitch = backgroundMusicSource.pitch;
        float elapsed = 0f;

        while (elapsed < pitchChangeTransitionDuration)
        {
            backgroundMusicSource.pitch = Mathf.Lerp(startPitch, targetPitch, elapsed / pitchChangeTransitionDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        backgroundMusicSource.pitch = targetPitch;
        isPitchChanged = !isPitchChanged;
    }

    private bool IsPlayerInAnyArea()
    {
        foreach (GameObject area in interactableAreas)
        {
            if (area.TryGetComponent<Collider2D>(out var areaCollider))
            {
                if (areaCollider.OverlapPoint(player.transform.position))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private IEnumerator FadeEffects()
    {
        StartCoroutine(ShakeAndZoom(shakeDuration));

        var startingValues = new
        {
            saturation = colorAdjustments.saturation.value,
            contrast = colorAdjustments.contrast.value,
            ca = chromaticAberration.intensity.value,
            vignette = vignette.intensity.value,
            filmGrain = filmGrain.intensity.value
        };

        bool[] toggles =
        {
            enableSaturation,
            enableContrast,
            enableChromaticAberration,
            enableVignette,
            enableFilmGrain
        };

        float[] targetValues =
        {
            toggles[0] ? (colorAdjustments.saturation.value == targetSaturation ? 0f : targetSaturation) : 0f,
            toggles[1] ? (colorAdjustments.contrast.value == targetContrast ? 0f : targetContrast) : 0f,
            toggles[2] ? (chromaticAberration.intensity.value == targetCAIntensity ? 0f : targetCAIntensity) : 0f,
            toggles[3] ? (vignette.intensity.value == targetVignetteIntensity ? 0f : targetVignetteIntensity) : 0f,
            toggles[4] ? (filmGrain.intensity.value == targetFilmGrainIntensity ? 0f : targetFilmGrainIntensity) : 0f
        };

        yield return new WaitForSeconds(effectDelay);

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

        colorAdjustments.saturation.value = targetValues[0];
        colorAdjustments.contrast.value = targetValues[1];
        chromaticAberration.intensity.value = targetValues[2];
        vignette.intensity.value = targetValues[3];
        filmGrain.intensity.value = targetValues[4];

        if (togglePlatforms)
        {
            TogglePlatforms();
        }
    }

    private void TogglePlatforms()
    {
        foreach (GameObject platform in platformsToToggle)
        {
            platform.SetActive(!platform.activeSelf);
        }
    }

    private IEnumerator ShakeAndZoom(float duration)
    {
        Vector3 originalPosition = transform.localPosition;
        float originalSize = targetCamera.orthographicSize;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float magnitude = Mathf.Lerp(0, shakeIntensity, elapsed / duration);
            transform.localPosition = originalPosition + new Vector3(Random.Range(-1f, 1f) * magnitude, Random.Range(-1f, 1f) * magnitude, 0);
            targetCamera.orthographicSize = Mathf.Lerp(originalSize, originalSize - zoomAmount, elapsed / zoomInDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
        elapsed = 0f;

        while (elapsed < zoomOutDuration)
        {
            targetCamera.orthographicSize = Mathf.Lerp(originalSize - zoomAmount, originalSize, elapsed / zoomOutDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        targetCamera.orthographicSize = originalSize;
    }
}
