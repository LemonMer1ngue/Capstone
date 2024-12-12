using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DeathController : MonoBehaviour
{
    public static DeathController Instance;

    public GameObject respawnAnimObject;
    public Animator anim;
    Vector2 checkpointPos;
    Rigidbody2D playerRb;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else
        {
            Destroy(gameObject);    
        }
    }

    void Start()
    {
        checkpointPos = transform.position;
        playerRb = GetComponent<Rigidbody2D>();
        respawnAnimObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            Die();
        }
    }

    public void UpdateCheckpoint(Vector3 pos)
    {
        checkpointPos = pos;
    } 

    void Die()
    {
        StartCoroutine(Respawn(1f));
    }

    IEnumerator Respawn(float duration)
    {
        playerRb.velocity = new Vector2(0, 0);
        respawnAnimObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        //yield return new WaitUntil(() => SceneManager.GetActiveScene().name == currentSceneName);
        anim.SetTrigger("Respawn");
        transform.position = checkpointPos;
        SaveSystem.Load();
        yield return new WaitForSeconds(duration);
        
        respawnAnimObject.SetActive(false);
       
       
    }

    #region Save and Load
    public void Save(ref PlayerSaveData data)
    {
        data.Position = transform.position;
    }

    public void Load(PlayerSaveData data)
    {
        transform.position = data.Position;
    }
    #endregion
}
[System.Serializable]
public struct PlayerSaveData
{
    public Vector3 Position;
}