using UnityEngine.SceneManagement;
using UnityEngine;

public class DimensionChanger : MonoBehaviour
{
    [SerializeField] private LevelConnection connection;
    [SerializeField] private string targetScenes;
    [SerializeField] private Transform spawnPlayer;

    private bool isPlayerInPortal = false;
    private GameObject boxInSceneA;

    void Start()
    {
        if (connection == LevelConnection.ActiveConnection)
        {
            var player = GameObject.FindWithTag("Player")?.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.transform.position = spawnPlayer.position;
            }
        }
    }

    void Update()
    {
        if (isPlayerInPortal && Input.GetKeyDown(KeyCode.E))
        {
            PortalManager.LastPortalUsed = spawnPlayer;
            LevelConnection.ActiveConnection = connection;
            SceneManager.LoadScene(targetScenes);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerMovement>();
        if (player != null)
        {
            isPlayerInPortal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerMovement>();
        if (player != null)
        {
            isPlayerInPortal = false;
        }
    }

    public static class PortalManager
    {
        public static Transform LastPortalUsed;
        public static string LastPortalID;
    }

}
