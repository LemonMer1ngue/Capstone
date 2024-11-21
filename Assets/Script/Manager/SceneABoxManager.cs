using UnityEngine;

public class SceneABoxManager : MonoBehaviour
{
    public GameObject[] boxesInSceneA;  

    void Start()
    {
        foreach (GameObject box in boxesInSceneA)
        {
            box.SetActive(BoxStatusManager.IsBoxActiveInSceneA);
        }
    }

    public void ActivateBoxesInSceneA()
    {
        BoxStatusManager.IsBoxActiveInSceneA = true;
        BoxStatusManager.IsBoxActiveInSceneB = false;
        foreach (GameObject box in boxesInSceneA)
        {
            box.SetActive(true);
        }
    }
}
