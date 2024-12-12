using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChapterSelect : MonoBehaviour
{
    public Button chapter1Button;
    public Button chapter2Button;
    public Button chapter3Button;
    public Button backButton;

    private int c1Scene;
    private int c2Scene;

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
        c1Scene = PlayerPrefs.GetInt("C1SavedScene");
        if (c1Scene != 0) 
        {
            SceneManager.LoadScene(c1Scene);
        }
        else
        {
            return;
        }
    
    }

    public void Chapter2()
    {
        c2Scene = PlayerPrefs.GetInt("C2SavedScene");
        if (c2Scene != 0)
        {
            SceneManager.LoadScene(c2Scene);
        }
        else
        {
            return;
        }
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
