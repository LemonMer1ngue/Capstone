using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteWiggle : MonoBehaviour
{
    public Volume volume; // Drag and drop the Volume component here
    public float wiggleSpeed = 5f; // Speed of the wiggle
    public float wiggleAmount = 0.2f; // Intensity range of the wiggle (added or subtracted)
    private Vignette vignette;

    private float baseIntensity; // Base intensity of the vignette

    void Start()
    {
        // Ensure the Volume has a Vignette effect
        if (volume.profile.TryGet<Vignette>(out vignette))
        {
            baseIntensity = vignette.intensity.value; // Save the initial intensity
        }
        else
        {
            Debug.LogError("No Vignette component found in the Volume profile.");
        }
    }

    void Update()
    {
        if (vignette != null)
        {
            // Calculate the new intensity value
            float newIntensity = baseIntensity + Mathf.Sin(Time.time * wiggleSpeed) * wiggleAmount;

            // Clamp the value between 0 and 1 (valid range for vignette intensity)
            newIntensity = Mathf.Clamp(newIntensity, 0f, 1f);

            // Apply the new intensity
            vignette.intensity.value = newIntensity;
        }
    }
}
