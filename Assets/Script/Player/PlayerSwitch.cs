using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitch : MonoBehaviour
{
    public PlayerMovement player1Movement;
    public CatMovement player2Movement;

    public Transform player1Transform;
    public Transform player2Transform;

    public Rigidbody2D player1Rigidbody;
    public Rigidbody2D player2Rigidbody;
    public EnhancedCameraFollow cameraFollow;

    public bool player1Activate;

    void Start()
    {
        // Pastikan Player1 aktif di awal
        player1Activate = true;
        player1Movement.enabled = true;
        player2Movement.enabled = false;

        player2Rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
        cameraFollow.SetTarget(player1Transform.transform);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightShift) && !player1Movement.IsMoving())
        {
            PlayerSwitching();
        }
    }

    void PlayerSwitching()
    {
        if (player1Activate)
        {
            // Matikan Player1, aktifkan Player2
            player1Movement.enabled = false;
            player1Rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

            player2Movement.enabled = true;
            player2Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

            cameraFollow.SetTarget(player2Transform.transform);
            player1Activate = false;
        }
        else
        {
            // Matikan Player2, aktifkan Player1
            player2Movement.enabled = false;
            player2Rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

            player1Movement.enabled = true;
            player1Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

            cameraFollow.SetTarget(player1Transform.transform);
            player1Activate = true;
        }
    }
}
