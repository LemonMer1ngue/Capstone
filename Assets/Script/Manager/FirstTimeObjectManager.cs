using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTimeObjectManager : MonoBehaviour
{
    private string key; // Key unik untuk GameObject ini

    private void Awake()
    {
        // Tetapkan key unik berdasarkan nama scene dan GameObject
        key = $"{gameObject.name}_Loaded_{UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}";

        // Periksa apakah ini pertama kali
        if (PlayerPrefs.GetInt(key, 0) == 0)
        {
            // Tandai sebagai sudah muncul
            PlayerPrefs.SetInt(key, 1);
            PlayerPrefs.Save();
        }
        else
        {
            // Hancurkan GameObject jika bukan pertama kali
            Destroy(gameObject);
        }
    }
}
