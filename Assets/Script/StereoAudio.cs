using UnityEngine;

public class StereoAudioEffect : MonoBehaviour
{
    public AudioSource audioSource;  // The Audio Source on the GameObject
    public Transform listener;        // The player or listener object

    [Header("Audio Distance Settings")]
    public float minDistance = 1f;   // Minimum distance to hear the audio
    public float maxDistance = 10f;   // Maximum distance to hear the audio

    private const float maxVolume = 0.1f; // Set maximum volume to 0.1
    private const float panMultiplier = 2f; // Multiplier for pan effect

    void Start()
    {
        DontDestroyOnLoad(listener.gameObject);
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Ensure audio starts playing
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    void Update()
    {
        // Check if the listener still exists
        if (listener == null)
        {
            // Try to find the listener (e.g., the player) in the scene
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                listener = player.transform; // Assign the player's transform as the listener
            }
            else
            {
                Debug.LogWarning("Listener object is missing and could not be found.");
                return; // Exit Update if listener is still not found
            }
        }

        // Ensure the audio is playing
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        // Calculate the direction to the listener
        Vector3 directionToListener = listener.position - transform.position;

        // Calculate the distance to the listener
        float distance = directionToListener.magnitude;
        if (distance > 0)
        {
            directionToListener.Normalize();

            // Adjust the panStereo property for a more noticeable stereo effect
            audioSource.panStereo = Mathf.Clamp(-directionToListener.x * panMultiplier, -1f, 1f); // Negated for correct panning

            // Adjust volume based on distance
            if (distance < minDistance)
            {
                audioSource.volume = maxVolume; // Full volume if within min distance
            }
            else if (distance > maxDistance)
            {
                audioSource.volume = 0f; // No volume if beyond max distance
            }
            else
            {
                // Scale volume based on distance between min and max
                audioSource.volume = Mathf.Clamp01(maxVolume * (1 - (distance - minDistance) / (maxDistance - minDistance)));
            }
        }
        else
        {
            // If the listener is at the same position, ensure max volume
            audioSource.volume = maxVolume;
        }
    }
}
