using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button ngButton;       // Reference for the Play button
    public Button chapterButton;
    public Button settingsButton;   // Reference for the Settings button
    public Button creditButton;
    public Button quitButton;       // Reference for the Exit button

    void Start()
    {
        // Add listeners to the buttons
        ngButton.onClick.AddListener(NewGame);
        chapterButton.onClick.AddListener(ChapterSelection);
        settingsButton.onClick.AddListener(OpenSettings);
        creditButton.onClick.AddListener(Credit);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void NewGame()
    {
        SceneManager.LoadScene("CutScene"); // Replace with your actual game scene name
    }

    public void ChapterSelection()
    {
        SceneManager.LoadScene("ChapterSelection");
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("SettingMenu");
    }

    public void Credit()
    {
        //SceneManager.LoadScene("Credit");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Exit Button Clicked");
    }
}
