using UnityEngine;

public class PressPlateController : MonoBehaviour
{
    [Header("Press Plate Settings")]
    public Transform pressPlate; // Referensi ke PressPlate
    public Vector3 pressedPositionOffset = new Vector3(0, -0.1f, 0); // Posisi turun saat diinjak
    public float moveSpeed = 2f; // Kecepatan pergerakan
    private Vector3 initialPosition; // Posisi awal PressPlate
    private int objectsOnPlate = 0; // Jumlah objek di atas plate

    void Start()
    {
        // Simpan posisi awal PressPlate
        initialPosition = pressPlate.localPosition;
    }

    void Update()
    {
        // Atur posisi target tergantung apakah ada objek yang menyentuh
        Vector3 targetPosition = (objectsOnPlate > 0) ? initialPosition + pressedPositionOffset : initialPosition;

        // Gerakkan PressPlate ke posisi target
        pressPlate.localPosition = Vector3.Lerp(pressPlate.localPosition, targetPosition, Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Tambah jumlah objek saat ada yang menyentuh
        if (other.CompareTag("Player") || other.CompareTag("InteractAble"))
        {
            objectsOnPlate++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Kurangi jumlah objek saat tidak ada yang menyentuh
        if (other.CompareTag("Player") || other.CompareTag("InteractAble"))
        {
            objectsOnPlate--;
        }
    }
}
