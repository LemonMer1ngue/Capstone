using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    public float transitionTime;
    public Animator transition;

    void Start()
    {
        if (Instance == null)
        {

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        //if (DialogueManager.Instance.isDialogueActive == false)
        //{
        //    LoadNextLevelTutorial();
        //}
    }

    public void LoadNextLevelTutorial()
    {
        StartCoroutine(LoadLevelTutorial());
    }

    IEnumerator LoadLevelTutorial()
    {
       
        transition.SetTrigger("Blink");
        yield return new WaitForSeconds(transitionTime);
        AsyncOperation operation = SceneManager.LoadSceneAsync("Tutorial");
        
        while (!operation.isDone)
        {
            Debug.Log(operation.progress);

            yield return null;
        }

        transition.SetTrigger("Awake");
    }
}
