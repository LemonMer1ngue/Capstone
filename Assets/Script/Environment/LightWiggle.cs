using UnityEngine;
using UnityEngine.Rendering.Universal; // Tambahkan ini

public class LightIntensityWiggle : MonoBehaviour
{
    public float wiggleAmount = 0.5f; // Amplitudo wiggle
    public float wiggleSpeed = 5f; // Kecepatan wiggle
    private Light2D light2D;
    private float originalIntensity;

    void Start()
    {
        light2D = GetComponent<Light2D>();
        originalIntensity = light2D.intensity;
    }

    void Update()
    {
        light2D.intensity = originalIntensity + Mathf.Sin(Time.time * wiggleSpeed) * wiggleAmount;
    }
}
