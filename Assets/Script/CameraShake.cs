using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public Camera camera; // Reference to the camera
    public float zoomAmount = 1f; // Amount to zoom in
    public float zoomInDuration = 0.5f; // Duration of the zoom-in effect (slow)
    public float zoomOutDuration = 0.2f; // Duration of the zoom-out effect (fast)
    public float shakeIntensity = 1f; // Maximum intensity of the shake

    public IEnumerator Shake(float duration)
    {
        Vector3 originalPosition = transform.localPosition; // Store the original position
        float originalSize = camera.orthographicSize; // Store the original orthographic size (for 2D)

        float elapsed = 0f;
        float elapsedZoomIn = 0f;
        float elapsedZoomOut = 0f;

        // Zoom in and shake simultaneously
        while (elapsed < duration)
        {
            // Gradually increase the magnitude for shaking
            float magnitude = Mathf.Lerp(0, shakeIntensity, elapsed / duration); // Increase over time

            // Generate random offset based on the current magnitude
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // Apply the shake offset
            transform.localPosition = originalPosition + new Vector3(x, y, 0);

            // Handle zoom-in
            if (elapsedZoomIn < zoomInDuration)
            {
                camera.orthographicSize = Mathf.Lerp(originalSize, originalSize - zoomAmount, elapsedZoomIn / zoomInDuration);
                elapsedZoomIn += Time.deltaTime;
            }
            else
            {
                camera.orthographicSize = originalSize - zoomAmount; // Ensure final zoom in size
            }

            elapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Reset to the original position after shaking
        transform.localPosition = originalPosition;

        // Zoom out quickly
        elapsedZoomOut = 0f;
        while (elapsedZoomOut < zoomOutDuration)
        {
            camera.orthographicSize = Mathf.Lerp(originalSize - zoomAmount, originalSize, elapsedZoomOut / zoomOutDuration);
            elapsedZoomOut += Time.deltaTime;
            yield return null;
        }

        camera.orthographicSize = originalSize; // Ensure we set back to the original size
    }
}
