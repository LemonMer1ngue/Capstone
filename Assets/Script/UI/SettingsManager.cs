using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    // Graphics Settings
    public Slider resolutionSlider;
    public Toggle fullscreenToggle;
    public Slider gammaSlider;

    // Audio Settings
    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;

    // Audio sources for SFX and music (ensure you assign them in Unity)
    public AudioSource sfxAudioSource;
    public AudioSource musicAudioSource;

    private void Start()
    {
        LoadSettings();  // Load saved settings when the scene starts
    }

    // Call this to save settings
    public void SaveSettings()
    {
        // Save graphics settings
        PlayerPrefs.SetFloat("Resolution", resolutionSlider.value);
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("Gamma", gammaSlider.value);

        // Save audio settings
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);

        PlayerPrefs.Save();
    }

    // Call this to load settings
    public void LoadSettings()
    {
        // Load graphics settings
        resolutionSlider.value = PlayerPrefs.GetFloat("Resolution", 1); // Default is 1 (max resolution)
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        gammaSlider.value = PlayerPrefs.GetFloat("Gamma", 1); // Default is 1 (neutral gamma)

        // Load audio settings
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1); // Default is 1 (full volume)
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1); // Default is 1 (full volume)
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1); // Default is 1 (full volume)

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
            Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, fullscreenToggle.isOn);
        }

        // Apply gamma by adjusting ambient light or a post-processing effect
        RenderSettings.ambientLight = Color.white * gammaSlider.value;
    }

    // Apply audio settings (Master, SFX, Music)
    private void ApplyAudioSettings()
    {
        // Set master volume globally (affects all audio)
        AudioListener.volume = masterVolumeSlider.value;

        // Adjust specific audio sources (SFX and Music)
        if (sfxAudioSource != null)
            sfxAudioSource.volume = sfxVolumeSlider.value;

        if (musicAudioSource != null)
            musicAudioSource.volume = musicVolumeSlider.value;
    }

    // When resolution slider is changed, apply the new settings
    public void OnResolutionChanged()
    {
        ApplyGraphicsSettings();
    }

    // When gamma slider is changed, apply the new gamma value
    public void OnGammaChanged()
    {
        ApplyGraphicsSettings();
    }

    // When fullscreen toggle is changed, apply the new fullscreen setting
    public void OnFullscreenToggle()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
        ApplyGraphicsSettings();
    }

    // When audio sliders are changed, apply new audio settings
    public void OnMasterVolumeChanged()
    {
        ApplyAudioSettings();
        Debug.Log("changed");
    }

    public void OnSFXVolumeChanged()
    {
        ApplyAudioSettings();
    }

    public void OnMusicVolumeChanged()
    {
        ApplyAudioSettings();
    }
}
