using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    public float transitionTime;
    public Animator transition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Update is called once per frame
    //void Update()
    //{
    //    //if (DialogueManager.Instance.isDialogueActive == false)
    //    //{
    //    //    LoadNextLevelTutorial();
    //    //}
    //}

    public void LoadNextLevelTutorial()
    {
        StartCoroutine(LoadLevelTutorial());
    }

    IEnumerator LoadLevelTutorial()
    {
       
        transition.SetTrigger("Blink");
        yield return new WaitForSeconds(transitionTime);
        AsyncOperation operation = SceneManager.LoadSceneAsync("Tutorial");
        transition.SetTrigger("Awake");
    }
}
