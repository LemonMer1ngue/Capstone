using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public Button backButton;
    public Button[] backsetButtons;
    public Button controlButton;
    public Button graphicsButton;
    public Button audioButton;

    public GameObject settingCanvas;

    public GameObject Settings;
    public GameObject Control;
    public GameObject Graphics;
    public GameObject Audio;

    void Start()
    {
        foreach (Button button in backsetButtons)
        {
            button.onClick.AddListener(BackSet);
        }

        backButton.onClick.AddListener(Back);
        controlButton.onClick.AddListener(ControlSetting);
        graphicsButton.onClick.AddListener(GraphicsSetting);
        audioButton.onClick.AddListener(AudioSetting);
        Settings.SetActive(true);
        Control.SetActive(false);
        Graphics.SetActive(false);
        Audio.SetActive(false);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Settings.activeSelf)
        {
            Back();
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            BackSet();
        }
    }


    public void ControlSetting()
    {
        Settings.SetActive(false);
        Control.SetActive(true);
        Graphics.SetActive(false);
        Audio.SetActive(false);
    }

    public void GraphicsSetting()
    {
        Settings.SetActive(false);
        Control.SetActive(false);
        Graphics.SetActive(true);
        Audio.SetActive(false);
    }

    public void AudioSetting()
    {
        Settings.SetActive(false);
        Control.SetActive(false);
        Graphics.SetActive(false);
        Audio.SetActive(true);
    }

    public void BackSet()
    {
        Settings.SetActive(true);
        Control.SetActive(false);
        Graphics.SetActive(false);
        Audio.SetActive(false);
    }
    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
