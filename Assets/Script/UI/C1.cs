using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class C1 : MonoBehaviour
{
    private string saveFilePath;

    private void Start()
    {
        saveFilePath = Application.persistentDataPath + "/save1.json";
    }

    // This method is called when another collider enters the trigger area
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger is the player (or any tag you set)
        if (other.CompareTag("Player"))  // Replace "Player" with the tag of your player object
        {
            SaveScene();
        }
    }

    public void SaveScene()
    {
        SaveData1 saveData = new SaveData1();
        saveData.c1SceneIndex = SceneManager.GetActiveScene().buildIndex;

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(saveFilePath, json);

        Debug.Log("Scene saved to JSON: " + json);
    }
}
