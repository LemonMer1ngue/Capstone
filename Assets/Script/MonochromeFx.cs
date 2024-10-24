using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System.Collections;

public class InteractableCameraEffect : MonoBehaviour
{
    public GameObject[] interactableAreas; // Array of interactable GameObjects
    public GameObject player; // Assign the player character GameObject in the inspector
    public GameObject[] platformsToToggle; // Assign the platforms to toggle in the inspector
    public float fadeDuration = 1f; // Duration for the fade effect
    public float shakeDuration = 0.2f; // Duration for the camera shake
    public float effectDelay = 0.1f; // Delay before applying the effects

    private PostProcessVolume volume;
    private ChromaticAberration CA;
    private ColorGrading colorGrading;
    private CameraShake cameraShake; // Reference to CameraShake

    private bool isCA = false;
    private bool isMonochrome = false;
    private bool isContrast = false;

    private Coroutine fadeCoroutine;

    void Start()
    {
        volume = Camera.main.GetComponent<PostProcessVolume>();
        colorGrading = volume.profile.GetSetting<ColorGrading>();
        CA = volume.profile.GetSetting<ChromaticAberration>();
        cameraShake = Camera.main.GetComponent<CameraShake>(); // Get CameraShake component

        cameraShake.camera = Camera.main; // Assign the main camera
    }

    void Update()
    {
        if (IsPlayerInAnyArea() && Input.GetKeyDown(KeyCode.E)) // Change key as needed
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(FadeEffects());
        }
    }

    private bool IsPlayerInAnyArea()
    {
        foreach (GameObject area in interactableAreas)
        {
            Collider2D areaCollider = area.GetComponent<Collider2D>();
            if (areaCollider != null && areaCollider.OverlapPoint(player.transform.position))
            {
                return true; // Player is in at least one area
            }
        }
        return false; // Player is not in any area
    }

    private IEnumerator FadeEffects()
    {
        // Start shaking the camera
        StartCoroutine(cameraShake.Shake(shakeDuration)); // Start the shake with public duration

        // Toggle states
        isMonochrome = !isMonochrome;
        isContrast = !isContrast;
        isCA = !isCA;

        // Prepare starting and target values
        float startSaturation = colorGrading.saturation.value;
        float targetSaturation = isMonochrome ? -100f : 0f;

        float startContrast = colorGrading.contrast.value;
        float targetContrast = isContrast ? 10f : 0f;

        float startCA = CA.intensity.value;
        float targetCA = isCA ? 1f : 0f;

        // Wait for the effect delay before starting to fade
        yield return new WaitForSeconds(effectDelay);

        float elapsedTime = 0f;

        // Fade in/out for each effect
        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;

            // Lerp values for fade effect
            colorGrading.saturation.value = Mathf.Lerp(startSaturation, targetSaturation, t);
            colorGrading.contrast.value = Mathf.Lerp(startContrast, targetContrast, t);
            CA.intensity.value = Mathf.Lerp(startCA, targetCA, t);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure final values are set
        colorGrading.saturation.value = targetSaturation;
        colorGrading.contrast.value = targetContrast;
        CA.intensity.value = targetCA;

        // Toggle platform visibility
        TogglePlatforms();
    }

    private void TogglePlatforms()
    {
        foreach (GameObject platform in platformsToToggle)
        {
            platform.SetActive(!platform.activeSelf); // Toggle visibility
        }
    }
}
