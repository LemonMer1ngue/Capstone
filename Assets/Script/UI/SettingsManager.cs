using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro; // For TextMeshPro

public class SettingsManager : MonoBehaviour
{
    // Graphics Settings
    [Header("Graphics")]
    public Slider resolutionSlider;
    public TextMeshProUGUI fullscreenText; // Text for the slider selector
    public Slider gammaSlider;

    // Audio Settings
    [Header("Audio")]
    public AudioMixer audioMixer;
    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;

    private string[] fullscreenOptions = { "Off", "On" }; // Options for the selector
    private int fullscreenIndex; // Tracks the current fullscreen option

    private void OnEnable()
    {
        // Ensure settings are loaded every time the settings menu is opened or when the scene is loaded.
        LoadSettings();
    }

    // Call this to save settings
    public void SaveSettings()
    {
        // Save graphics settings to PlayerPrefs
        PlayerPrefs.SetFloat("Resolution", resolutionSlider.value);
        PlayerPrefs.SetInt("Fullscreen", fullscreenIndex); // Save fullscreen option
        PlayerPrefs.SetFloat("Gamma", gammaSlider.value);

        // Save audio settings to PlayerPrefs
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);

        PlayerPrefs.Save(); // Save immediately
    }

    // Call this to load settings
    private void LoadSettings()
    {
        // Load graphics settings from PlayerPrefs
        resolutionSlider.value = PlayerPrefs.GetFloat("Resolution", resolutionSlider.value); // Default if not found
        fullscreenIndex = PlayerPrefs.GetInt("Fullscreen", 0); // Default fullscreen option is "Off"
        Screen.fullScreen = fullscreenIndex == 1; // Apply fullscreen setting

        // Update fullscreen text immediately after loading the setting
        UpdateFullscreenDisplay();

        gammaSlider.value = PlayerPrefs.GetFloat("Gamma", gammaSlider.value); // Default if not found

        // Load audio settings from PlayerPrefs
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", masterVolumeSlider.value); // Default if not found
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", sfxVolumeSlider.value); // Default if not found
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", musicVolumeSlider.value); // Default if not found

        ApplyGraphicsSettings(); // Apply graphics settings immediately
        ApplyAudioSettings(); // Apply audio settings immediately
    }

    // Apply graphics settings (Resolution, Fullscreen, Gamma)
    private void ApplyGraphicsSettings()
    {
        int resolutionIndex = Mathf.FloorToInt(resolutionSlider.value);
        Resolution[] resolutions = Screen.resolutions;

        // Ensure we're within the available resolutions
        if (resolutionIndex >= 0 && resolutionIndex < resolutions.Length)
        {
            // Change resolution dynamically based on slider value
            Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, fullscreenIndex == 1);
        }

        // Apply gamma by adjusting ambient light or a post-processing effect
        RenderSettings.ambientLight = Color.white * gammaSlider.value;
    }

    // Apply audio settings (Master, SFX, Music)
    private void ApplyAudioSettings()
    {
        // Set master volume globally (affects all audio)
        audioMixer.SetFloat("MasterVolume", masterVolumeSlider.value);
        audioMixer.SetFloat("SFXVolume", sfxVolumeSlider.value);
        audioMixer.SetFloat("MusicVolume", musicVolumeSlider.value);
    }

    // When fullscreen selector's left button is clicked
    public void OnFullscreenLeft()
    {
        fullscreenIndex = (fullscreenIndex - 1 + fullscreenOptions.Length) % fullscreenOptions.Length;
        Screen.fullScreen = fullscreenIndex == 1; // Apply the new fullscreen setting
        UpdateFullscreenDisplay();
    }

    // When fullscreen selector's right button is clicked
    public void OnFullscreenRight()
    {
        fullscreenIndex = (fullscreenIndex + 1) % fullscreenOptions.Length;
        Screen.fullScreen = fullscreenIndex == 1; // Apply the new fullscreen setting
        UpdateFullscreenDisplay();
    }

    // Update the text display for the fullscreen selector
    private void UpdateFullscreenDisplay()
    {
        fullscreenText.text = fullscreenOptions[fullscreenIndex];
        fullscreenText.ForceMeshUpdate(); // Ensure TMP text updates
    }

    // When resolution slider is changed, apply the new settings
    public void OnResolutionChanged()
    {
        ApplyGraphicsSettings();
        SaveSettings(); // Save the setting when the resolution slider changes
    }

    // When gamma slider is changed, apply the new gamma value
    public void OnGammaChanged()
    {
        ApplyGraphicsSettings();
        SaveSettings(); // Save the setting when the gamma slider changes
    }

    // When audio sliders are changed, apply new audio settings
    public void OnMasterVolumeChanged()
    {
        ApplyAudioSettings();
        SaveSettings(); // Save the setting when the master volume slider changes
    }

    public void OnSFXVolumeChanged()
    {
        ApplyAudioSettings();
        SaveSettings(); // Save the setting when the SFX volume slider changes
    }

    public void OnMusicVolumeChanged()
    {
        ApplyAudioSettings();
        SaveSettings(); // Save the setting when the music volume slider changes
    }
}
