using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button playButton;       // Reference for the Play button
    public Button settingsButton;   // Reference for the Settings button
    public Button exitButton;       // Reference for the Exit button

    void Start()
    {
        // Add listeners to the buttons
        playButton.onClick.AddListener(PlayGame);
        settingsButton.onClick.AddListener(OpenSettings);
        exitButton.onClick.AddListener(ExitGame);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("CutScene"); // Replace with your actual game scene name
    }

    public void OpenSettings()
    {
/*        // Load your settings scene or logic here
        Debug.Log("Settings Button Clicked");
        SceneManager.LoadScene("SettingsMenu"); // Replace with your actual settings scene name*/
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit Button Clicked");
    }
}
