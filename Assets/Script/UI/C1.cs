using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class C1 : MonoBehaviour
{
    private int c1SceneIndex;

    public void C1Save()
    {
        c1SceneIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("C1SavedScene", c1SceneIndex);
    }

    // Update is called once per frame
    void Update()
    {
        C1Save();
    }
}
