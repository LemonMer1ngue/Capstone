using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteRandomWiggle : MonoBehaviour
{
    public Volume volume; // Drag and drop the Volume component here
    public float wiggleSpeedMin = 1f; // Minimum speed of the wiggle
    public float wiggleSpeedMax = 10f; // Maximum speed of the wiggle
    public float wiggleAmount = 0.2f; // Intensity range of the wiggle (added or subtracted)
    private Vignette vignette;

    private float baseIntensity; // Base intensity of the vignette
    private float currentSpeed; // Current speed of the wiggle
    private float nextSpeedChangeTime; // When to change the speed next

    void Start()
    {
        // Ensure the Volume has a Vignette effect
        if (volume.profile.TryGet<Vignette>(out vignette))
        {
            baseIntensity = vignette.intensity.value; // Save the initial intensity
            currentSpeed = Random.Range(wiggleSpeedMin, wiggleSpeedMax); // Initialize with a random speed
            nextSpeedChangeTime = Time.time + Random.Range(0.5f, 2f); // Set initial time for speed change
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
            // Change speed at random intervals
            if (Time.time >= nextSpeedChangeTime)
            {
                currentSpeed = Random.Range(wiggleSpeedMin, wiggleSpeedMax);
                nextSpeedChangeTime = Time.time + Random.Range(0.5f, 2f); // Schedule next speed change
            }

            // Calculate the new intensity value
            float noise = Mathf.PerlinNoise(Time.time * currentSpeed, 0f); // Generate smooth random value
            float newIntensity = baseIntensity + (noise - 0.5f) * 2f * wiggleAmount;

            // Clamp the value between 0 and 1 (valid range for vignette intensity)
            newIntensity = Mathf.Clamp(newIntensity, 0f, 1f);

            // Apply the new intensity
            vignette.intensity.value = newIntensity;
        }
    }
}
