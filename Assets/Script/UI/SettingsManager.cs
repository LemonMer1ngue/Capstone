using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro; // For TextMeshPro

public class SettingsManager : MonoBehaviour
{
    // Graphics Settings
    [Header("Graphics")]
/*    public Slider resolutionSlider;*/
    public TextMeshProUGUI fullscreenText;
/*    public Slider gammaSlider;*/

    // Audio Settings
    [Header("Audio")]
    public AudioMixer audioMixer;
    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;

    private string[] fullscreenOptions = { "Off", "On" };
    private int fullscreenIndex;

    private void Update()
    {

    }

    private void OnEnable()
    {
        LoadSettings(); // Load settings when menu is opened
    }

    // Save all settings to PlayerPrefs
    public void SaveSettings()
    {
/*        PlayerPrefs.SetFloat("Resolution", resolutionSlider.value);*/
        PlayerPrefs.SetInt("Fullscreen", fullscreenIndex);
/*        PlayerPrefs.SetFloat("Gamma", gammaSlider.value);*/
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
        PlayerPrefs.Save(); // Save immediately
    }

    // Load settings from PlayerPrefs
    private void LoadSettings()
    {
/*        resolutionSlider.value = PlayerPrefs.GetFloat("Resolution", resolutionSlider.value);*/
        fullscreenIndex = PlayerPrefs.GetInt("Fullscreen", 0);
        Screen.fullScreen = fullscreenIndex == 1;

        UpdateFullscreenDisplay();
/*        gammaSlider.value = PlayerPrefs.GetFloat("Gamma", gammaSlider.value);*/
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", masterVolumeSlider.value);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", sfxVolumeSlider.value);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", musicVolumeSlider.value);

/*        ApplyGraphicsSettings();*/
        ApplyAudioSettings();
    }

    // Apply graphics settings (Resolution, Fullscreen, Gamma)
/*    private void ApplyGraphicsSettings()
    {
        int resolutionIndex = Mathf.FloorToInt(resolutionSlider.value);
        Resolution[] resolutions = Screen.resolutions;

        if (resolutionIndex >= 0 && resolutionIndex < resolutions.Length)
        {
            Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, fullscreenIndex == 1);
        }

        RenderSettings.ambientLight = Color.white * gammaSlider.value;
    }*/

    // Apply audio settings (Master, SFX, Music)
    private void ApplyAudioSettings()
    {
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



    // Update the text display for fullscreen
    private void UpdateFullscreenDisplay()
    {
        fullscreenText.text = fullscreenOptions[fullscreenIndex];
        fullscreenText.ForceMeshUpdate(); // Ensure TMP text updates
    }

/*    // Handlers for UI sliders
    public void OnResolutionChanged()
    {
        ApplyGraphicsSettings();
        SaveSettings();
    }

    public void OnGammaChanged()
    {
        ApplyGraphicsSettings();
        SaveSettings();
    }
*/
    public void OnMasterVolumeChanged()
    {
        ApplyAudioSettings();
        SaveSettings();
    }

    public void OnSFXVolumeChanged()
    {
        ApplyAudioSettings();
        SaveSettings();
    }

    public void OnMusicVolumeChanged()
    {
        ApplyAudioSettings();
        SaveSettings();
    }

    // Call this function when you want to check the values in PlayerPrefs


}
