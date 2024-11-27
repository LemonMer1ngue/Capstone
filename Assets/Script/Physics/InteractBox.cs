using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBox : MonoBehaviour
{
    public int idBox;
    public bool beingPushed; // Apakah box sedang didorong
    public bool isPlayerNearby; // Apakah player dekat dengan box
    private Rigidbody2D rb;
    public bool hasBeenActivated = false;
    private Vector2 initialPosition; // Posisi awal box

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        beingPushed = false;
        isPlayerNearby = false;

        // Inisialisasi posisi awal box
        initialPosition = transform.position;

        // Set posisi box ke posisi awal
        transform.position = initialPosition;
    }

    void FixedUpdate()
    {
        // Jika player dekat, freeze posisi X untuk mencegah tergelincir
        if (!beingPushed && isPlayerNearby)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        // Jika box sedang didorong, atau player menjauh, lepas semua pembekuan
        else if (beingPushed || !isPlayerNearby)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Biarkan hanya rotasi yang dibatasi
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Jika player mendekat, aktifkan penguncian posisi X
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Jika player menjauh, lepas penguncian posisi X
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}
