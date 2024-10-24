using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        // Mendapatkan Rigidbody2D dari GameObject
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Mendapatkan input dari tombol arah atau WASD
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Mengatur kecepatan dan arah
        Vector2 movement = new Vector2(moveX, moveY);

        // Menetapkan kecepatan ke Rigidbody2D
        rb.velocity = movement * moveSpeed;
    }
}
