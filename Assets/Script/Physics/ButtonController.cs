using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    // Daftar platform dan wall yang dikontrol tombol ini
    [SerializeField] private List<MovingPlatform> controlledPlatforms;
    [SerializeField] private List<WallsMovement> controlledWalls;

    private bool isPressed = false; // Status tombol

    // Ketika tombol ditekan oleh Player atau Box
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("InteractAble"))
        {
            isPressed = true;  // Tombol ditekan
            UpdateControlledObjects(); // Update status pada semua objek yang dikontrol
        }
    }

    // Ketika tombol dilepaskan
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("InteractAble"))
        {
            isPressed = false;  // Tombol dilepas
            UpdateControlledObjects();  // Update status pada semua objek yang dikontrol
        }
    }

    // Update status semua objek yang dikontrol
    private void UpdateControlledObjects()
    {
        // Update status MovingPlatform
        foreach (MovingPlatform platform in controlledPlatforms)
        {
            if (platform != null)
            {
                platform.SetButtonPressed(isPressed);  // Mengubah status isButtonPressed pada platform
            }
        }

        // Update status MovingWalls
        foreach (WallsMovement wall in controlledWalls)
        {
            if (wall != null)
            {
                wall.SetButtonPressed(isPressed);  // Mengubah status isButtonPressed pada wall
            }
        }
    }

    // Menambahkan platform untuk dikontrol oleh tombol ini
    public void AddPlatform(MovingPlatform platform)
    {
        if (!controlledPlatforms.Contains(platform))
        {
            controlledPlatforms.Add(platform);
        }
    }

    // Menghapus platform dari kontrol tombol ini
    public void RemovePlatform(MovingPlatform platform)
    {
        if (controlledPlatforms.Contains(platform))
        {
            controlledPlatforms.Remove(platform);
        }
    }

    // Menambahkan wall untuk dikontrol oleh tombol ini
    public void AddWall(WallsMovement wall)
    {
        if (!controlledWalls.Contains(wall))
        {
            controlledWalls.Add(wall);
        }
    }

    // Menghapus wall dari kontrol tombol ini
    public void RemoveWall(WallsMovement wall)
    {
        if (controlledWalls.Contains(wall))
        {
            controlledWalls.Remove(wall);
        }
    }
}