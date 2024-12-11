using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GammaSlider : MonoBehaviour
{
    [SerializeField] private Slider gammaSlider; // Assign in Inspector

    private Volume volume;
    private LiftGammaGain liftGammaGain;

    private void Start()
    {
        // Find the Volume in the scene
        volume = FindObjectOfType<Volume>();
        if (volume == null)
        {
            Debug.LogError("No Volume found in the scene.");
            return;
        }

        // Try to get the LiftGammaGain override
        if (!volume.profile.TryGet(out liftGammaGain))
        {
            Debug.LogError("Lift, Gamma, Gain override not found in the Volume Profile.");
            return;
        }

        // Initialize the slider with a default value
        gammaSlider.value =0f;    // Neutral gamma
        gammaSlider.onValueChanged.AddListener(OnGammaChanged);

        Debug.Log("Gamma Slider initialized.");
    }

    private void OnGammaChanged(float value)
    {
        if (liftGammaGain != null)
        {
            // Maintain the alpha value and only adjust RGB
            Vector4 currentGamma = liftGammaGain.gamma.value;
            liftGammaGain.gamma.value = new Vector4(0.1f, 0.1f, 0.1f, value);
            Debug.Log($"Gamma adjusted to: {value}");
        }
    }
}
