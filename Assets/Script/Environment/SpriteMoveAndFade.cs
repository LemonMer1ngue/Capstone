using UnityEngine;

public class SpriteMoveAndFade : MonoBehaviour
{
    public GameObject targetSprite; // Sprite yang akan dipindahkan
    public Vector3 targetPosition; // Posisi tujuan
    public float moveDuration = 1f; // Durasi perpindahan
    public float holdDuration = 2f; // Durasi bertahan di alpha 1
    public float fadeDuration = 1f; // Durasi fade-in/fade-out

    private SpriteRenderer spriteRenderer;
    private bool isMoving = false;
    private Collider2D triggerCollider;

    void Start()
    {
        if (targetSprite != null)
        {
            spriteRenderer = targetSprite.GetComponent<SpriteRenderer>();

            // Pastikan SpriteRenderer ada dan atur alpha awal ke 0
            if (spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                color.a = 0f;
                spriteRenderer.color = color;
            }
        }

        // Simpan referensi collider trigger
        triggerCollider = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isMoving)
        {
            Debug.Log("Player menyentuh trigger, memulai animasi!");
            StartMove();

            // Nonaktifkan collider trigger agar tidak terpicu kembali
            if (triggerCollider != null)
            {
                triggerCollider.enabled = false;
            }
        }
    }

    public void StartMove()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveAndFade());
        }
    }

    private System.Collections.IEnumerator MoveAndFade()
    {
        if (spriteRenderer == null) yield break;

        isMoving = true;

        Vector3 startPosition = targetSprite.transform.position;
        float elapsedTime = 0f;

        // Fade-in dan Pindah ke targetPosition
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;

            // Interpolasi posisi
            targetSprite.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);

            // Interpolasi alpha
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            UpdateAlpha(alpha);

            yield return null;
        }

        // Pastikan sprite berada di targetPosition dan alpha 1
        targetSprite.transform.position = targetPosition;
        UpdateAlpha(1f);

        // Tahan di alpha 1
        yield return new WaitForSeconds(holdDuration);

        // Fade-out
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            // Interpolasi alpha
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            UpdateAlpha(alpha);

            yield return null;
        }

        // Pastikan alpha kembali ke 0
        UpdateAlpha(0f);

        // Hancurkan targetSprite
        if (targetSprite != null)
        {
            Destroy(targetSprite);
            Debug.Log("Target Sprite dihancurkan.");
        }

        isMoving = false;
    }

    private void UpdateAlpha(float alpha)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}
