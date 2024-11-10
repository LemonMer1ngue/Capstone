using UnityEngine.SceneManagement;
using UnityEngine;

public class PortalInteraction : MonoBehaviour
{
    public string targetScene;
    public Transform portalExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerMovement>();
        if (player != null && player.isHoldingBox )
        {
            if (targetScene == "Real_01")
            {
                BoxStatusManager.IsBoxActiveInSceneA = true;
                BoxStatusManager.IsBoxActiveInSceneB = false;
            }
            else if (targetScene == "Fake_01")
            {
                BoxStatusManager.IsBoxActiveInSceneA = false;
                BoxStatusManager.IsBoxActiveInSceneB = true;
            }

            SceneManager.LoadScene(targetScene);
        }
    }
}
