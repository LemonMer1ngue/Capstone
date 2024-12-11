using UnityEngine;

public class CanvasFade : MonoBehaviour
{
    public GameObject targetObject; // Objek target (Canvas)
    public float fadeDuration = 1f; // Durasi fade-in/fade-out
    public float displayDuration = 3f; // Durasi tampilan sebelum fade-out

    private CanvasGroup canvasGroup;
    private bool isFading = false;
    private Collider2D triggerCollider;

    void Start()
    {
        if (targetObject != null)
        {
            // Mendapatkan komponen CanvasGroup dari target object
            canvasGroup = targetObject.GetComponent<CanvasGroup>();

            // Pastikan CanvasGroup ada dan atur alpha awal ke 0
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                targetObject.SetActive(false); // Target mulai dalam keadaan nonaktif
            }
        }

        // Simpan referensi collider trigger
        triggerCollider = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isFading)
        {
            Debug.Log("Player menyentuh trigger, memulai fade-in!");
            targetObject.SetActive(true); // Aktifkan target object
            StartCoroutine(FadeCanvas(1f)); // Fade-in (alpha ke 1)

            if (triggerCollider != null)
            {
                triggerCollider.enabled = false; // Nonaktifkan collider trigger
            }

            // Mulai timer untuk fade-out setelah durasi tertentu
            Invoke(nameof(StartFadeOut), displayDuration);
        }
    }

    private void StartFadeOut()
    {
        if (!isFading)
        {
            Debug.Log("Timer selesai, memulai fade-out!");
            StartCoroutine(FadeCanvas(0f)); // Fade-out (alpha ke 0)
        }
    }

    private System.Collections.IEnumerator FadeCanvas(float targetAlpha)
    {
        if (canvasGroup == null) yield break; // Jika tidak ada CanvasGroup, hentikan eksekusi

        isFading = true;
        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha; // Pastikan nilai alpha sesuai target

        // Nonaktifkan target object jika alpha mencapai 0
        if (targetAlpha == 0f)
        {
            targetObject.SetActive(false);
        }

        isFading = false;
    }
}
