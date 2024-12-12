using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GammaSettings : MonoBehaviour
{
    public GameObject gammaCanvas;
    public GameObject settingCanvas;
    public Button backButton;



    private void Start()
    {
        gammaCanvas.SetActive(false);
        backButton.onClick.AddListener(Back);
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

    public void Back()
    {
        gammaCanvas.SetActive(false);
        settingCanvas.SetActive(true);

    }
}
