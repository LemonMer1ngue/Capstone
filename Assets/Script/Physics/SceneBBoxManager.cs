using UnityEngine;

public class SceneBBoxManager : MonoBehaviour
{
    public GameObject[] boxInSceneB;

    void Start()
    {
        foreach (GameObject box in boxInSceneB)
        {
            box.SetActive(BoxStatusManager.IsBoxActiveInSceneB);
        }
    }

    public void ActivateBoxInSceneB()
    {
        BoxStatusManager.IsBoxActiveInSceneB = true;
        BoxStatusManager.IsBoxActiveInSceneA = false;
        foreach (GameObject box in boxInSceneB)
        {
            box.SetActive(true);
        }
    }
}
