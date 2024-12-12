using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    public AudioSource audioSource; // AudioSource untuk memutar suara
    public AudioClip[] stoneClips;  // Suara langkah di batu
    public AudioClip[] grassClips;  // Suara langkah di rumput
    public string currentSurface = "Grass"; // Default permukaan

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not assigned!");
        }
    }

    void Update()
    {
        if (characterController != null && characterController.isGrounded && characterController.velocity.magnitude > 0.1f)
        {
            if (!audioSource.isPlaying)
            {
                PlayFootstepSound();
            }
        }
    }

    void PlayFootstepSound()
    {
        AudioClip clip = null;

        // Pilih klip berdasarkan permukaan saat ini
        switch (currentSurface)
        {
            case "Stone":
                clip = GetRandomClip(stoneClips);
                break;
            case "Grass":
                clip = GetRandomClip(grassClips);
                break;
            default:
                Debug.LogWarning("Unknown surface type!");
                break;
        }

        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.pitch = Random.Range(0.9f, 1.1f); // Variasi nada
            audioSource.volume = Random.Range(0.8f, 1.0f); // Variasi volume
            audioSource.Play();
        }
    }

    AudioClip GetRandomClip(AudioClip[] clips)
    {
        if (clips.Length == 0) return null;
        return clips[Random.Range(0, clips.Length)];
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ubah jenis permukaan berdasarkan tag atau nama zona
        if (other.CompareTag("Stone"))
        {
            currentSurface = "Stone";
        }
        else if (other.CompareTag("Grass"))
        {
            currentSurface = "Grass";
        }
    }
}
