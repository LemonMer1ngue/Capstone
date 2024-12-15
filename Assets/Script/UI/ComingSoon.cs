using UnityEngine;
using UnityEngine.SceneManagement;

public class ComingSoon : MonoBehaviour
{
    public GameObject comingSoonCanvas;

    void Start()
    {
        if (comingSoonCanvas != null)
        {
            comingSoonCanvas.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            if (comingSoonCanvas != null)
            {
                comingSoonCanvas.SetActive(true);
                Invoke(nameof(LoadCreditScene), 5f);
            }
        }
    }

    private void LoadCreditScene()
    {
        SceneManager.LoadScene("Credit");
    }
}