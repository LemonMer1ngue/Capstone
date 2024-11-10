using UnityEngine;
using UnityEngine.SceneManagement;

public class BoxManager : MonoBehaviour
{
    public GameObject boxInSceneA;  // Kotak di Scene A
    public GameObject boxInSceneB;  // Kotak di Scene B

    private void Start()
    {
        // Panggil fungsi untuk aktifkan/nonaktifkan kotak saat scene mulai
        ManageBoxActivation();
    }

    // Fungsi untuk mengaktifkan kotak berdasarkan lokasi terakhir kotak
    private void ManageBoxActivation()
    {
        // Ambil data lokasi kotak (0 untuk Scene A, 1 untuk Scene B)
        int boxLocation = PlayerPrefs.GetInt("BoxLocation", 0);

        if (SceneManager.GetActiveScene().name == "Real_01")
        {
            boxInSceneA.SetActive(boxLocation == 0);  // Aktifkan kotak di Scene A
            boxInSceneB.SetActive(false);             // Nonaktifkan kotak di Scene B
        }
        else if (SceneManager.GetActiveScene().name == "Fake_01")
        {
            boxInSceneA.SetActive(false);             // Nonaktifkan kotak di Scene A
            boxInSceneB.SetActive(boxLocation == 1);  // Aktifkan kotak di Scene B
        }
    }

    // Fungsi yang dipanggil saat player pindah scene
    public void MoveBoxToScene(string targetScene)
    {
        if (targetScene == "Real_01")
        {
            PlayerPrefs.SetInt("BoxLocation", 0);  // Simpan bahwa kotak ada di Scene A
        }
        else if (targetScene == "Fake_01")
        {
            PlayerPrefs.SetInt("BoxLocation", 1);  // Simpan bahwa kotak ada di Scene B
        }

        SceneManager.LoadScene(targetScene);  // Pindah ke scene tujuan
    }
}
