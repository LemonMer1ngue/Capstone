using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    // Graphics Settings
    [Header("Graphics")]
    public TextMeshProUGUI fullscreenText;
    public TextMeshProUGUI resolutionText;
    public Volume volume;

    // Audio Settings
    [Header("Audio")]
    public AudioMixer audioMixer;
    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;

    // Gamma Settings
    [Header("Gamma")]
    public Slider gammaSlider; // New gamma slider

    private LiftGammaGain liftGammaGain;

    private string[] fullscreenOptions = { "Off", "On" };
    private int fullscreenIndex;

    private (int width, int height)[] customResolutions = {
        (1920, 1080), // Full HD
        (1024, 768),
        (1280, 1024),
        (1366, 768),
        (1440, 900),
        (1600, 900),
        (1680, 1050)
    };
    private int resolutionIndex;

    private void OnEnable()
    {
        InitializeVolume(); // Initialize volume for gamma
        LoadSettings(); // Load settings when menu is opened
    }

    public void SaveSettings()
    {
        // Save fullscreen and resolution settings
        PlayerPrefs.SetInt("Fullscreen", fullscreenIndex);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);

        // Save audio settings
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);

        // Save gamma setting
        PlayerPrefs.SetFloat("Gamma", gammaSlider.value);

        PlayerPrefs.Save(); // Save immediately
        Debug.Log("Settings Saved.");
    }

    public void LoadSettings()
    {
        // Load fullscreen and resolution settings
        fullscreenIndex = PlayerPrefs.GetInt("Fullscreen", 0);
        resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);

        Screen.fullScreen = fullscreenIndex == 1;
        ApplyResolutionSetting();

        // Load audio settings
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

        masterVolumeSlider.value = masterVolume;
        sfxVolumeSlider.value = sfxVolume;
        musicVolumeSlider.value = musicVolume;

        ApplyAudioSettings();

        // Load gamma setting
        float gammaValue = PlayerPrefs.GetFloat("Gamma", 1.0f);
        gammaSlider.value = gammaValue;
        ApplyGammaSetting(gammaValue);

        // Update UI
        UpdateFullscreenDisplay();
        UpdateResolutionDisplay();

        Debug.Log("Settings Loaded.");
    }

    public void InitializeVolume()
    {
        volume.profile.TryGet(out liftGammaGain);
        gammaSlider.onValueChanged.AddListener(OnGammaChanged);
    }

    private void ApplyGammaSetting(float value)
    {
        if (liftGammaGain != null)
        {
            liftGammaGain.gamma.value = new Vector4(0.1f, 0.1f, 0.1f, value);
            Debug.Log($"Gamma Applied: {value}");
        }
    }

    private void OnGammaChanged(float value)
    {
        ApplyGammaSetting(value);
    }

    private void ApplyResolutionSetting()
    {
        (int width, int height) resolution = customResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void ApplyAudioSettings()
    {
        audioMixer.SetFloat("MasterVolume", masterVolumeSlider.value);
        audioMixer.SetFloat("SFXVolume", sfxVolumeSlider.value);
        audioMixer.SetFloat("MusicVolume", musicVolumeSlider.value);
    }

    private void UpdateFullscreenDisplay()
    {
        fullscreenText.text = fullscreenOptions[fullscreenIndex];
        fullscreenText.ForceMeshUpdate();
    }

    private void UpdateResolutionDisplay()
    {
        (int width, int height) resolution = customResolutions[resolutionIndex];
        resolutionText.text = $"{resolution.width} x {resolution.height}";
        resolutionText.ForceMeshUpdate();
    }

    public void OnFullscreenLeft()
    {
        fullscreenIndex = (fullscreenIndex - 1 + fullscreenOptions.Length) % fullscreenOptions.Length;
        Screen.fullScreen = fullscreenIndex == 1;
        UpdateFullscreenDisplay();
    }

    public void OnFullscreenRight()
    {
        fullscreenIndex = (fullscreenIndex + 1) % fullscreenOptions.Length;
        Screen.fullScreen = fullscreenIndex == 1;
        UpdateFullscreenDisplay();
    }

    public void OnResolutionLeft()
    {
        resolutionIndex = (resolutionIndex - 1 + customResolutions.Length) % customResolutions.Length;
        ApplyResolutionSetting();
        UpdateResolutionDisplay();
    }

    public void OnResolutionRight()
    {
        resolutionIndex = (resolutionIndex + 1) % customResolutions.Length;
        ApplyResolutionSetting();
        UpdateResolutionDisplay();
    }

    public void OnMasterVolumeChanged()
    {
        ApplyAudioSettings();
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
