using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgDepanKamera : MonoBehaviour
{
    private void Awake()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            // Menentukan posisi di depan kamera
            Vector3 newPosition = mainCamera.transform.position;
            newPosition.z = 0; // Pastikan objek berada pada sumbu Z layer 2D

            // Memindahkan GameObject ini ke posisi baru
            transform.position = newPosition;
        }
        else
        {
            Debug.LogWarning("Main Camera not found!");
        }
    }

    private void Update()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            // Menentukan posisi di depan kamera
            Vector3 newPosition = mainCamera.transform.position;
            newPosition.z = 0; // Pastikan objek berada pada sumbu Z layer 2D

            // Memindahkan GameObject ini ke posisi baru
            transform.position = newPosition;
        }
        else
        {
            Debug.LogWarning("Main Camera not found!");
        }
    }
}

