using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueHandler : MonoBehaviour
{
    public GameObject continueButton;
    private int continueScene;

    private void Start()
    {
        CheckSave();
    }
    private void CheckSave()
    {
        continueScene = PlayerPrefs.GetInt("SavedScene");
        if (continueScene != 0)
        {
            continueButton.SetActive(true);
        }
        else 
        { 
            continueButton.SetActive(false); 
        }
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(continueScene);
    }
}
