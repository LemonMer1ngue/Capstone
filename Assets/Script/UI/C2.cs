using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class C2 : MonoBehaviour
{
    private int c2SceneIndex;

    public void C2Save()
    {
        c2SceneIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("C2SavedScene", c2SceneIndex);
    }

    // Update is called once per frame
    void Update()
    {
        C2Save();
    }
}
