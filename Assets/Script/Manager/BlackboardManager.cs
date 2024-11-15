using System.Collections.Generic;
using UnityEngine;

public class BlackboardManager : MonoBehaviour
{
    private Dictionary<string, Vector3> objectPositions = new Dictionary<string, Vector3>();
    private Dictionary<string, bool> objectMergeStatus = new Dictionary<string, bool>();  

    public static BlackboardManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveObjectPosition(string objectID, Vector3 position)
    {
        if (objectPositions.ContainsKey(objectID))
        {
            objectPositions[objectID] = position;
        }
        else
        {
            objectPositions.Add(objectID, position);
        }
    }

    public Vector3 LoadObjectPosition(string objectID)
    {
        if (objectPositions.ContainsKey(objectID))
        {
            return objectPositions[objectID];
        }

        return Vector3.zero;  
    }

    public void SaveObjectMergeStatus(string objectID, bool isMerged)
    {
        if (objectMergeStatus.ContainsKey(objectID))
        {
            objectMergeStatus[objectID] = isMerged;
        }
        else
        {
            objectMergeStatus.Add(objectID, isMerged);
        }
    }

    public bool LoadObjectMergeStatus(string objectID)
    {
        if (objectMergeStatus.ContainsKey(objectID))
        {
            return objectMergeStatus[objectID];
        }

        return false;  
    }
}
