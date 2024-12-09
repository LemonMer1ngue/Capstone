using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay = 3f; // Waktu sebelum platform jatuh
    // [SerializeField] private float destroyDelay = 0.1f; // Waktu sebelum platform dihancurkan
    // [SerializeField] private Rigidbody2D rb; // Rigidbody2D platform
    // [SerializeField] private float respawnDelay = 2f;

    public HingeJoint2D hingeJoint;
    private Vector3 initialPosition; // Posisi awal platform
    private Transform initialParent; // Parent awal platform dalam hierarchy
    private bool isFalling;
    private float time; // Status apakah platform sedang jatuh

    void Start()
    {
        hingeJoint = GetComponent<HingeJoint2D>();
    }

    private void Update()
    {

    }



    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }


    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        hingeJoint.enabled = false;
    }


}

// void Start()
//     {
//         // Simpan posisi awal dan parent platform
//         initialPosition = transform.position;
//         initialParent = transform.parent;

//         // Pastikan platform siap digunakan
//         ResetPlatform();
//     }

//     private void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (collision.gameObject.CompareTag("Player") && !isFalling)
//         {
//             StartCoroutine(Fall());
//         }
//     }

//     private IEnumerator Fall()
//     {
//         isFalling = true; // Tandai platform sedang jatuh
//         yield return new WaitForSeconds(fallDelay);

//         rb.bodyType = RigidbodyType2D.Dynamic; // Ubah ke Dynamic untuk mulai jatuh
//         yield return new WaitForSeconds(destroyDelay);

//         yield return new WaitForSeconds(respawnDelay);

//         RespawnPlatform(); // Respawn platform setelah hancur
//         Destroy(gameObject); // Hancurkan platform lama
//     }

//     private void RespawnPlatform()
//     {
//         // Ambil prefab dari FallingPlatformManager
//         GameObject prefab = FallingPlatformManager.Instance.GetFallingPlatformPrefab();
//         if (prefab != null)
//         {
//             // Instansiasi platform baru di posisi awal dengan parent yang sama
//             GameObject newPlatform = Instantiate(prefab, initialPosition, Quaternion.identity, initialParent);

//             // Reset platform baru agar siap digunakan kembali
//             FallingPlatform newFallingPlatform = newPlatform.GetComponent<FallingPlatform>();
//             if (newFallingPlatform != null)
//             {
//                 newFallingPlatform.Initialize(initialPosition, initialParent);
//             }
//         }
//         else
//         {
//             Debug.LogError("Failed to respawn platform: Prefab not found!");
//         }
//     }

//     public void Initialize(Vector3 position, Transform parent)
//     {
//         // Set ulang data platform saat respawn
//         initialPosition = position;
//         initialParent = parent;

//         // Reset platform agar siap digunakan kembali
//         ResetPlatform();
//     }

//     public void ResetPlatform()
//     {
//         // Reset Rigidbody ke Kinematic dan hentikan pergerakan
//         rb.bodyType = RigidbodyType2D.Kinematic;
//         rb.velocity = Vector2.zero;

//         // Aktifkan collider dan tandai siap digunakan
//         Collider2D collider = GetComponent<Collider2D>();
//         if (collider != null)
//         {
//             collider.enabled = true;
//         }

//         isFalling = false; // Tandai platform siap digunakan kembali
//     }
