using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveScene : MonoBehaviour
{
    private int currentSceneIndex;

    public void Save()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("SavedScene", currentSceneIndex);
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        Save();
    }
}
