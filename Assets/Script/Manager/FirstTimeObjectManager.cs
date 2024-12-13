using UnityEngine;

public class FirstTimeObjectManager : MonoBehaviour
{
    private static FirstTimeObjectManager instance;

    void Awake()
    {
        if (instance == null)
        {
           instance = this;
            DontDestroyOnLoad(gameObject); // Optional jika ingin memastikan objek tidak dihancurkan saat pindah scene
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
