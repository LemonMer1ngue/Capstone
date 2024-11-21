using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class Dialog : MonoBehaviour
{
    public GameObject dialogBox;
    public TextMeshProUGUI dialogText;
    public string[] dialogLine;
    public float dialogSpeed;
    private int index;
    public float fadeSpeed = 1f;
    public ParticleSystem[] smokeParticle;
    public Image blackPanel;
    void Start()
    {
        dialogText.text = string.Empty;
        StartDialog();
        SoundManager.instance.PlayBreathingSound();

    }

    void StartDialog()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator TypeLine()
    {
        while (index < dialogLine.Length)
        {
            Debug.Log("Memulai menampilkan dialog: " + dialogLine[index]);
            if (index == 1)
            {
                SoundManager.instance.PlayWhisperSound();
            }

            foreach (char c in dialogLine[index].ToCharArray())
            {
                dialogText.text += c;
                yield return new WaitForSeconds(dialogSpeed);
            }
            yield return new WaitForSeconds(5f);
            dialogText.text = string.Empty;
            dialogBox.SetActive(false);

            if (index == 1)
            {
                yield return new WaitForSeconds(1f);
                SoundManager.instance.PlayLaughSound();
            }

            switch (index)
            {
                case 0:
                    SoundManager.instance.StopBreathingSound();

                    yield return StartCoroutine(PlayHeartBeatAndDarken());
                    break;
            }
            index++;
        }
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Tutorial");
    }

    IEnumerator PlayWhisperSound()
    {
        Debug.Log("Memulai suara bisikan");
        SoundManager.instance.PlayWhisperSound();
        yield return new WaitForSeconds(5f);

    }

    IEnumerator PlayHeartBeatAndDarken()
    {
        SoundManager.instance.PlayHeartBeatSound();
        foreach (var particle in smokeParticle)
        {
            particle.Play();
        }
        yield return new WaitForSeconds(3f);

        float alpha = 0;

        while (alpha < 1)
        {
            alpha += Time.deltaTime * (fadeSpeed * 0.5f);
            blackPanel.color = new Color(0, 0, 0, alpha);
            if (alpha > 0.2f)
            {
                foreach (var particle in smokeParticle)
                {
                    particle.Stop();
                }
            }

            yield return null;
        }
    }


}
