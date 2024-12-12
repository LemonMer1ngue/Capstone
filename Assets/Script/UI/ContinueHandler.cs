using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;


public class ContinueHandler : MonoBehaviour
{
    public Button continueButton;
    public GameObject test;
    private string saveFilePath;

    private void Start()
    {
        saveFilePath = Application.persistentDataPath + "/save.json";

        CheckSave();
        continueButton.onClick.AddListener(ContinueGame);
    }
    private void CheckSave()
    {
        if (File.Exists(saveFilePath))
        {
            test.SetActive(true);
        }
        else 
        {
            test.SetActive(false);
        }
    }

    public void ContinueGame()
    {
        string json = File.ReadAllText(saveFilePath);
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);

        Debug.Log("Loading Scene Index from JSON: " + saveData.savedSceneIndex);
        SceneManager.LoadScene(saveData.savedSceneIndex);
    }
}
