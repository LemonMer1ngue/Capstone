using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class ChapterSelect : MonoBehaviour
{
    public Button chapter1Button;
    public Button chapter2Button;
    public Button chapter3Button;
    public Button backButton;

    private string saveFilePath1;
    private string saveFilePath2;

    void Start()
    {
        saveFilePath1 = Application.persistentDataPath + "/save1.json";
        saveFilePath2 = Application.persistentDataPath + "/save2.json";
        // Add listeners to the buttons
        chapter1Button.onClick.AddListener(Chapter1);
        chapter2Button.onClick.AddListener(Chapter2);
        chapter3Button.onClick.AddListener(Chapter3);
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
        if (File.Exists(saveFilePath1))
        {
            string json = File.ReadAllText(saveFilePath1);
            SaveData1 saveData1 = JsonUtility.FromJson<SaveData1>(json);

            Debug.Log("Loading Scene Index from JSON: " + saveData1.c1SceneIndex);
            SceneManager.LoadScene(saveData1.c1SceneIndex);
        }
        else
        {
            return;
        }

    }

    public void Chapter2()
    {
        if (File.Exists(saveFilePath2))
        {
            string json = File.ReadAllText(saveFilePath2);
            SaveData2 saveData2 = JsonUtility.FromJson<SaveData2>(json);

            Debug.Log("Loading Scene Index from JSON: " + saveData2.c2SceneIndex);
            SceneManager.LoadScene(saveData2.c2SceneIndex);
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
