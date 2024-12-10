using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsMovement : MonoBehaviour
{
    public Transform posA, posB;

    private float speed = 2f;
    private bool isButtonPressed = false;
    private Vector2 targetPos;

    void Start()
    {
        targetPos = posA.position; // WallGate dimulai di posisi awal
    }

    void Update()
    {
        // Tentukan target posisi berdasarkan status tombol
        targetPos = isButtonPressed ? posB.position : posA.position;

        // Pindahkan objek secara bertahap menuju target posisi
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    public void SetButtonPressed(bool pressed)
    {
        // Mengatur status isButtonPressed berdasarkan argumen pressed
        isButtonPressed = pressed;
    }
}
