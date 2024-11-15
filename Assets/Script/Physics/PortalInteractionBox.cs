using UnityEngine.SceneManagement;
using UnityEngine;

public class PortalInteraction : MonoBehaviour
{
    public string targetScene;  
    public Transform portalExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerMovement>();
        if (player != null && player.isHoldingBox)
        {
            UpdateBoxStatus(targetScene);
            SceneManager.LoadScene(targetScene);
        }
    }

    private void UpdateBoxStatus(string sceneName)
    {
        if (sceneName.StartsWith("Real"))
        {
            BoxStatusManager.IsBoxActiveInSceneA = true;
            BoxStatusManager.IsBoxActiveInSceneB = false;
        }
        else if (sceneName.StartsWith("Fake"))
        {
            BoxStatusManager.IsBoxActiveInSceneA = false;
            BoxStatusManager.IsBoxActiveInSceneB = true;
        }
    }
}
