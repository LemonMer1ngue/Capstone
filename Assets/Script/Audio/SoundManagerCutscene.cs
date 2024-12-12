using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManagerCutscene : MonoBehaviour
{
    [Header("Audio")]
    public GameObject beachWaves;
    public GameObject beachWind;
    public GameObject suspence;
    

    [Header("GameObject")]
    public GameObject dialogBox;
    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "CutScene")
        {
            beachWaves.SetActive(true);
            beachWind.SetActive(true);
        }
    }
    void Start()
    {
        
    }

   
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "CutScene")
        {
            beachWaves.SetActive(false);
            beachWind.SetActive(false);
            suspence.SetActive(false);
            
        }

        if (dialogBox != null && !dialogBox.activeSelf)
        {
            suspence.SetActive(true);
            
        }
        
    }
}
