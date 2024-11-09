using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public static BoxManager Instance;
    public GameObject Box;

    void Awake()
    {
        // Cek apakah ada instance BoxManager lain
        if (Instance != null)
        {
            Destroy(this.gameObject);  // Hapus instance baru yang muncul
            return;
        }

        // Tentukan Instance BoxManager
        Instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);

        // Cek apakah Box sudah ada di scene
        if (Box != null && GameObject.Find(Box.name) != null)
        {
            // Hancurkan objek Box jika sudah ada di scene
            Destroy(Box);
            Debug.Log("Box sudah ada di scene, menghancurkan yang baru.");
        }
    }
}
