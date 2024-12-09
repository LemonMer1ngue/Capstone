using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMovement : MonoBehaviour
{
    public Transform posA, posB;

    private float speed = 2f;
    private bool isButtonPressed = false;
    private Vector2 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        targetPos = posA.position; // Platform dimulai di posisi awal

    }

    // Update is called once per frame
    void Update()
    {
        targetPos = isButtonPressed ? posB.position : posA.position;
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    public void SetButtonPressed(bool pressed)
    {
        isButtonPressed = pressed;
    }
}
