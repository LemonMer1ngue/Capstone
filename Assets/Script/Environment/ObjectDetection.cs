using UnityEngine;

public class MoveTargetObjectOnTrigger : MonoBehaviour
{
    public GameObject targetObject; // Objek yang akan bergerak (ditetapkan di Inspector)
    public float targetY = 10f; // Posisi Y target objek yang ingin dicapai
    public float moveSpeed = 2f; // Kecepatan pergerakan objek
    private bool moveObjectUp = false; // Flag untuk pergerakan naik
    private bool moveObjectDown = false; // Flag untuk pergerakan turun
    private Vector3 initialPosition; // Posisi awal objek target

    void Start()
    {
        // Menyimpan posisi awal objek target
        if (targetObject != null)
        {
            initialPosition = targetObject.transform.position;
        }
    }

    void Update()
    {
        // Jika objek mulai bergerak ke atas, lakukan pergerakan secara bertahap
        if (moveObjectUp && targetObject != null)
        {
            float newY = Mathf.MoveTowards(targetObject.transform.position.y, targetY, moveSpeed * Time.deltaTime);
            targetObject.transform.position = new Vector3(targetObject.transform.position.x, newY, targetObject.transform.position.z);

            // Jika objek sudah mencapai target Y, berhenti bergerak ke atas
            if (targetObject.transform.position.y == targetY)
            {
                moveObjectUp = false;
                Debug.Log("Objek telah mencapai posisi target!");
            }
        }

        // Jika objek mulai bergerak ke bawah (kembali ke posisi awal), lakukan pergerakan mundur secara bertahap
        if (moveObjectDown && targetObject != null)
        {
            float newY = Mathf.MoveTowards(targetObject.transform.position.y, initialPosition.y, moveSpeed * Time.deltaTime);
            targetObject.transform.position = new Vector3(targetObject.transform.position.x, newY, targetObject.transform.position.z);

            // Jika objek sudah kembali ke posisi awal, berhenti bergerak ke bawah
            if (targetObject.transform.position.y == initialPosition.y)
            {
                moveObjectDown = false;
                Debug.Log("Objek kembali ke posisi awal!");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Memeriksa apakah objek yang menyentuh trigger adalah pemain (misalnya, tag "Player")
        if (other.CompareTag("Player"))
        {
            // Aktifkan target objek jika belum aktif
            if (targetObject != null && !targetObject.activeSelf)
            {
                targetObject.SetActive(true);
                Debug.Log("Target object diaktifkan!");
            }

            // Mulai pergerakan objek menuju posisi target (naik)
            moveObjectUp = true;
            moveObjectDown = false; // Pastikan pergerakan turun dihentikan saat trigger pertama kali dipicu
            Debug.Log("Player menyentuh trigger, objek mulai bergerak ke atas!");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Memeriksa apakah objek yang keluar dari trigger adalah pemain (misalnya, tag "Player")
        if (other.CompareTag("Player"))
        {
            // Mulai pergerakan objek kembali ke posisi awal (ke bawah)
            moveObjectDown = true;
            moveObjectUp = false; // Pastikan pergerakan naik dihentikan saat pemain keluar trigger
            Debug.Log("Player keluar dari trigger, objek mulai kembali ke posisi awal!");
        }
    }

    // Untuk menggambar trigger area di Scene (untuk debugging)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(2f, 2f, 2f)); // Ukuran trigger area (misalnya 2x2x2)
    }
}
