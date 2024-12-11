using UnityEngine;

public class CanvasUnactive : MonoBehaviour
{
    public Canvas canvas; 

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player")) 
        {
            canvas.gameObject.SetActive(false);
        }
    }
}
