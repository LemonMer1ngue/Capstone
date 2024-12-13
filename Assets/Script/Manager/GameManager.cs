using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return null;
            }
            if (instance == null)
            {
                Instantiate(Resources.Load<GameManager>("GameManager"));
            }
#endif
            return instance;
        }
    }

    public DeathController Death { get; set; }
    public CatMovement CatMovement { get; set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else
        {
            Destroy(gameObject);
        }

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Death = player.GetComponent<DeathController>();
            if (Death == null)
            {
                Debug.LogError("DeathController tidak ditemukan pada object player!");
            }
        }
        else
        {
            Debug.LogError("Player tidak ditemukan di scene!");
        }
    }

    private void OnEnable()
    {
        DeathController.OnPlayerInitialized += UpdatePlayerReference;
    }

    private void OnDisable()
    {
        DeathController.OnPlayerInitialized -= UpdatePlayerReference;
    }

    private void UpdatePlayerReference(GameObject player)
    {
        Death = player.GetComponent<DeathController>();
        if (Death == null)
        {
            Debug.LogError("DeathController tidak ditemukan pada object player!");
        }
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        SaveSystem.Save();
    //    }

    //    if (Input.GetKeyDown(KeyCode.X))
    //    {
    //        SaveSystem.Load();
    //    }
    //}
}