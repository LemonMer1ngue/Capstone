using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSystem : MonoBehaviour
{
    public static LoadSystem instance;

    public GameObject respawnAnimObject;
    public Animator anim;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        respawnAnimObject.SetActive(false);
    }

    public void Respawn()
    {
        StartCoroutine(Load(1f));
    }

    IEnumerator Load(float duration)
    {
        respawnAnimObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        yield return new WaitForSeconds(duration);
        anim.SetTrigger("Respawn");
        //transform.position = checkpointPos;
        SaveSystem.Load();
        yield return new WaitForSeconds(duration);

        respawnAnimObject.SetActive(false);
    }
}
