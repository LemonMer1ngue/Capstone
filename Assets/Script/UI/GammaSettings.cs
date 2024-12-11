using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GammaSettings : MonoBehaviour
{
    public GameObject gammaCanvas;
    public GameObject settingCanvas;

    private void Start()
    {
        gammaCanvas.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        GoBack();
    }

   public void GoBack()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            gammaCanvas.SetActive(false);
            settingCanvas.SetActive(true);
        }

    }
}
