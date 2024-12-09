using UnityEngine;

public class TestTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger activated by: " + other.name); // Menampilkan nama objek yang memasuki trigger
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("Trigger staying: " + other.name); // Menampilkan nama objek yang masih berada di dalam trigger
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger exited by: " + other.name); // Menampilkan nama objek yang keluar dari trigger
    }
}
