using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MonochromeEffectSRP : MonoBehaviour
{
    // Reference to the Post-Processing Volume
    private PostProcessVolume volume;
    private ColorGrading colorGrading;
    private bool isMonochrome = false;
    private bool isContrast = false;

    void Start()
    {
        // Try to get the PostProcessVolume component attached to the camera or in the scene
        volume = GetComponent<PostProcessVolume>();

        if (volume == null)
        {
            Debug.LogError("No PostProcessVolume component found! Please add a PostProcessVolume to the camera.");
            return;
        }

        // Check if the ColorGrading effect exists in the PostProcessVolume profile
        if (!volume.profile.TryGetSettings(out colorGrading))
        {
            Debug.LogError("No Color Grading effect found in the PostProcessVolume profile!");
            return;
        }

        Debug.Log("Color Grading effect successfully found.");
    }

    void Update()
    {
        // Toggle monochrome effect when the player presses the M key
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("M key pressed.");
            ToggleMonochrome();
            ToggleContrast();
        }
    }

    void ToggleMonochrome()
    {
        if (colorGrading == null) return;

        isMonochrome = !isMonochrome;

        if (isMonochrome)
        {
            Debug.Log("Setting monochrome effect.");
            // Set saturation to -100 to make the screen monochrome
            colorGrading.saturation.value = -100f;
        }
        else
        {
            Debug.Log("Restoring color.");
            // Set saturation back to 0 to restore normal color
            colorGrading.saturation.value = 0f;
        }
    }

    void ToggleContrast()
    {
        if (colorGrading == null) return;

        isContrast = !isContrast;

        if (isContrast)
        {
            // Set contrats to 20 to make the screen monochrome
            colorGrading.contrast.value = 10f;
        }
        else
        {
            // Set saturation back to 0 to restore normal color
            colorGrading.contrast.value = 0f;
        }
    }
}
