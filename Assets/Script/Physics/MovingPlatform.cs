using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform posA, posB; // Posisi awal dan akhir platform
    public float speed; // Kecepatan platform
    private Vector2 targetPos; // Posisi target platform
    private Vector2 platformVelocity; // Kecepatan platform untuk player
    private Vector2 lastPlatformPosition; // Posisi terakhir platform
    private bool isButtonPressed = false;
    private Rigidbody2D playerRb;
    private bool onPlatform; // Status tombol untuk platform

    public bool alwaysMoving;

    void Start()
    {
        targetPos = posA.position; // Platform dimulai di posisi awal
        lastPlatformPosition = transform.position;
    }

    void Update()
    {
        if (alwaysMoving)
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
        else
        {
            if (isButtonPressed)
            {
                targetPos = posB.position; // Jika tombol ditekan, bergerak ke posB
            }
            else
            {
                targetPos = posA.position; // Jika tombol dilepas, kembali ke posA
            }

            // Gerakkan platform
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            // Hitung velocity platform
            platformVelocity = ((Vector2)transform.position - lastPlatformPosition) / Time.deltaTime;
            lastPlatformPosition = transform.position;
        }

    }

    private void FixedUpdate()
    {
        if (isButtonPressed || alwaysMoving)
        {
            // Gerakkan platform ke posisi target
            if (onPlatform && playerRb != null)
            {
                // Menambahkan kecepatan platform ke velocity player
                playerRb.velocity += platformVelocity; // Mengatur kecepatan horizontal
            }
        }

        // Hitung velocity platform
        // platformVelocity = ((Vector2)transform.position - lastPlatformPosition) / Time.deltaTime;
        // lastPlatformPosition = transform.position;


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onPlatform = true;
            playerRb = collision.GetComponent<Rigidbody2D>();
            lastPlatformPosition = transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onPlatform = false;
            playerRb = null;
        }
    }


    // Method untuk mengatur status tombol
    public void SetButtonPressed(bool pressed)
    {
        isButtonPressed = pressed;
    }


}