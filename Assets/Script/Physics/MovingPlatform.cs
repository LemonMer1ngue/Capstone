using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform posA, posB;
    public float speed;
    private Vector2 targetPos;
    private Vector2 platformVelocity;
    private Vector2 lastPlatformPosition;
    private bool onPlatform;
    private Rigidbody2D playerRb;

    void Start()
    {
        targetPos = posB.position;
        lastPlatformPosition = transform.position;
    }

    void Update()
    {
        // Move the platform between posA and posB
        if (Vector2.Distance(transform.position, posA.position) < .1f)
        {
            targetPos = posB.position;
        }
        else if (Vector2.Distance(transform.position, posB.position) < .1f)
        {
            targetPos = posA.position;
        }
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // Calculate platform's current velocity based on position change
        platformVelocity = ((Vector2)transform.position - lastPlatformPosition) / Time.deltaTime;
        lastPlatformPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (onPlatform && playerRb != null)
        {
            // Apply platform's velocity to the player
            playerRb.velocity += platformVelocity;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onPlatform = true;
            playerRb = collision.GetComponent<Rigidbody2D>();
            lastPlatformPosition = transform.position;  // Reset position tracking for accurate velocity
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onPlatform = false;
            playerRb = null;  // Clear reference to player's Rigidbody on exit
        }
    }
}
