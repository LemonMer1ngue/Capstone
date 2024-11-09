using UnityEditor.MemoryProfiler;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DimensionChanger : MonoBehaviour
{
    [SerializeField] private LevelConnection connection;
    [SerializeField] private string targetDimensions;
    [SerializeField] private Transform spawnPlayer;
    [SerializeField] private string portalID;

    private bool isPlayerInPortal = false;
    private GameObject boxInSceneA;

    void Start()
    {
        if (PortalManager.LastPortalID == portalID && connection == LevelConnection.ActiveConnection)
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
            PortalManager.LastPortalID = portalID;
            PortalManager.LastPortalUsed = spawnPlayer;
            LevelConnection.ActiveConnection = connection;
            SceneManager.LoadScene(targetDimensions);
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
