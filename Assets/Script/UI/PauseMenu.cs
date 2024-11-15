using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu; // Reference to the pause menu (the parent)
    public Button resumeButton; // Button to resume the game
    public Button mainMenuButton; // Button to go to the main menu
    public Button exitButton; // Button to exit the game
    public Button settingsButton; // Button to open settings

    public static bool IsPaused { get; private set; } // Static variable for paused state

    void Start()
    {
        // Ensure the pause menu is not visible at the start
        foreach (Transform child in pauseMenu.transform)
        {
            child.gameObject.SetActive(false);
        }

        // Add button listeners
        resumeButton.onClick.AddListener(ResumeGame);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        exitButton.onClick.AddListener(ExitGame);
        settingsButton.onClick.AddListener(OpenSettings);
    }

    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused) ResumeGame();
            else PauseGame();
        }
    }

    void PauseGame()
    {
        // Show the pause menu
        foreach (Transform child in pauseMenu.transform)
        {
            child.gameObject.SetActive(true);
        }
        Time.timeScale = 0f; // Pause the game
        IsPaused = true; // Update the paused state
    }

    void ResumeGame()
    {
        // Hide the pause menu
        foreach (Transform child in pauseMenu.transform)
        {
            child.gameObject.SetActive(false);
        }
        Time.timeScale = 1f; // Resume the game
        IsPaused = false; // Update the paused state
    }

    void GoToMainMenu()
    {
        Time.timeScale = 1f; // Ensure the game is unpaused
        IsPaused = false; // Explicitly set paused state to false
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
    }

    void OpenSettings()
    {
        // Open settings logic goes here
    }
    void ExitGame()
    {
        Application.Quit(); // Quit the application
    }
}
