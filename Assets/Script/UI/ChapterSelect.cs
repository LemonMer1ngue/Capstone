using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChapterSelect : MonoBehaviour
{
    public Button chapter1Button;
    public Button chapter2Button;
    public Button chapter3Button;
    public Button backButton;

    void Start()
    {
        // Add listeners to the buttons
        chapter1Button.onClick.AddListener(Chapter1);
        chapter1Button.onClick.AddListener(Chapter2);
        chapter1Button.onClick.AddListener(Chapter3);
        backButton.onClick.AddListener(Back);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }

    public void Chapter1()
    {
        //SceneManager.LoadScene("");
    }

    public void Chapter2()
    {
        //SceneManager.LoadScene("");
    }

    public void Chapter3()
    {
        //SceneManager.LoadScene("");
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
