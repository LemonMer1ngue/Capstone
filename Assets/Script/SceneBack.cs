using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneBack : MonoBehaviour
{
    public string targetScene = "MainMenu"; // Nama scene yang ingin dituju
    public Button backButton; // Referensi ke tombol yang akan diaktifkan

    [SerializeField]
    private float waitTime = 10f; // Waktu tunggu yang bisa diatur di Inspector

    private bool isBackFunctionActive = false; // Status untuk mengontrol aktif atau tidaknya fungsi Back

    void Start()
    {
        if (backButton != null)
        {
            backButton.interactable = false; // Setel tombol tidak aktif pada awalnya
            StartCoroutine(ActivateButtonAfterDelay(waitTime)); // Mulai coroutine untuk mengaktifkan tombol setelah waktu tunggu
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isBackFunctionActive)
        {
            Back();
        }
    }

    public void Back()
    {
        SceneManager.LoadScene(targetScene);
    }

    private IEnumerator ActivateButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Tunggu selama waktu yang ditentukan
        if (backButton != null)
        {
            backButton.interactable = true; // Aktifkan tombol
        }
        isBackFunctionActive = true; // Setel status fungsi Back menjadi aktif
    }
}
