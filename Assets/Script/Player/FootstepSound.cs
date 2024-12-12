using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    public LayerMask groundLayer; // Assign this in Inspector
    public float raycastDistance = 0.5f;
    public float raycastOriginOffset = 0.1f;

    [Header("Footstep Sounds")]
    public List<AudioClip> grassFS; // Grass Footsteps
    public List<AudioClip> rockFS;  // Rock Footsteps
    public List<AudioClip> woodFS;  // Wood Footsteps
    public List<AudioClip> dirtFS;  // Dirt Footsteps
    public List<AudioClip> jellyFS; // Jelly Footsteps
    public AudioSource footstepSource;
    public float footstepInterval = 0.5f; // Interval between footsteps

    [Header("Random Pitch Range")]
    public float pitchMin = 0.9f; // Minimum pitch value
    public float pitchMax = 1.1f; // Maximum pitch value

    [Header("Random Volume Range")]
    public float volumeMin = 0.8f; // Minimum volume value
    public float volumeMax = 1.0f; // Maximum volume value

    private Rigidbody2D rb;
    private bool isMoving = false;
    private float footstepTimer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody2D found on this GameObject!");
        }
    }

    private void Update()
    {
        // Check if the player is moving
        isMoving = rb.velocity.magnitude > 0.1f;

        if (isMoving)
        {
            // Handle footstep sound
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0)
            {
                PlayFootstepSound();
                footstepTimer = footstepInterval; // Reset timer
            }
        }
    }

    private void PlayFootstepSound()
    {
        Vector2 rayOrigin = transform.position + new Vector3(0, -raycastOriginOffset, 0);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, raycastDistance, groundLayer);

        // Debug visual ray
        Debug.DrawRay(rayOrigin, Vector2.down * raycastDistance, Color.blue);

        if (hit.collider != null)
        {
            Debug.Log($"Hit object: {hit.collider.gameObject.name}, Tag: {hit.collider.tag}");
            string groundTag = hit.collider.tag;

            AudioClip clipToPlay = null;

            // Determine which sound to play based on the tag
            if (groundTag == "Grass" && grassFS.Count > 0)
            {
                clipToPlay = grassFS[Random.Range(0, grassFS.Count)];
            }
            else if (groundTag == "Rock" && rockFS.Count > 0)
            {
                clipToPlay = rockFS[Random.Range(0, rockFS.Count)];
            }
            else if (groundTag == "Wood" && woodFS.Count > 0)
            {
                clipToPlay = woodFS[Random.Range(0, woodFS.Count)];
            }
            else if (groundTag == "Dirt" && dirtFS.Count > 0)
            {
                clipToPlay = dirtFS[Random.Range(0, dirtFS.Count)];
            }
            else if (groundTag == "Jelly" && jellyFS.Count > 0)
            {
                clipToPlay = jellyFS[Random.Range(0, jellyFS.Count)];
            }

            if (clipToPlay != null)
            {
                // Randomize pitch and volume
                footstepSource.pitch = Random.Range(pitchMin, pitchMax);
                footstepSource.volume = Random.Range(volumeMin, volumeMax);

                // Play the randomly selected footstep sound
                footstepSource.PlayOneShot(clipToPlay);
            }
        }
    }
}
