using UnityEngine;

public class SceneABoxManager : MonoBehaviour
{
    public GameObject boxInSceneA;

    void Start()
    {
        boxInSceneA.SetActive(BoxStatusManager.IsBoxActiveInSceneA);
    }

    public void ActivateBoxInSceneA()
    {
        BoxStatusManager.IsBoxActiveInSceneA = true;
        BoxStatusManager.IsBoxActiveInSceneB = false;
        boxInSceneA.SetActive(true);
    }
}
