using UnityEngine;

public class BoxStatus : MonoBehaviour
{
    private string objectID;

    void Start()
    {
        objectID = gameObject.name;

        Vector3 savedPosition = BlackboardManager.instance.LoadObjectPosition(objectID);

        if (savedPosition != Vector3.zero)
        {
            transform.position = savedPosition;
        }
    }

    void Update()
    {
        BlackboardManager.instance.SaveObjectPosition(objectID, transform.position);
    }
}
