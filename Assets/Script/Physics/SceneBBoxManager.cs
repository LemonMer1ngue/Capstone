using UnityEngine;

public class SceneBBoxManager : MonoBehaviour
{
    public GameObject boxInSceneB;

    void Start()
    {
        boxInSceneB.SetActive(BoxStatusManager.IsBoxActiveInSceneB);
    }

    public void ActivateBoxInSceneB()
    {
        BoxStatusManager.IsBoxActiveInSceneA = false;
        BoxStatusManager.IsBoxActiveInSceneB = true;
        boxInSceneB.SetActive(true);  
    }
}
