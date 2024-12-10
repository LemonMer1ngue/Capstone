using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    // Daftar platform dan wall yang dikontrol tombol ini
    [SerializeField] private List<GameObject> controlledPlatforms;
    [SerializeField] private List<GameObject> controlledWalls;

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
        foreach (GameObject obj in controlledPlatforms)
        {
            MovingPlatform platform = obj.GetComponent<MovingPlatform>();
            if (platform != null)
            {
                platform.SetButtonPressed(isPressed);
            }
        }

        foreach (GameObject obj in controlledWalls)
        {
            WallsMovement wall = obj.GetComponent<WallsMovement>();
            if (wall != null)
            {
                wall.SetButtonPressed(isPressed);
            }
        }
    }

    // Menambahkan platform untuk dikontrol oleh tombol ini
    public void AddPlatform(GameObject platform)
    {
        if (!controlledPlatforms.Contains(platform))
        {
            controlledPlatforms.Add(platform);
        }
    }

    // Menghapus platform dari kontrol tombol ini
    public void RemovePlatform(GameObject platform)
    {
        if (controlledPlatforms.Contains(platform))
        {
            controlledPlatforms.Remove(platform);
        }
    }

    // Menambahkan wall untuk dikontrol oleh tombol ini
    public void AddWall(GameObject wall)
    {
        if (!controlledWalls.Contains(wall))
        {
            controlledWalls.Add(wall);
        }
    }

    // Menghapus wall dari kontrol tombol ini
    public void RemoveWall(GameObject wall)
    {
        if (controlledWalls.Contains(wall))
        {
            controlledWalls.Remove(wall);
        }
    }
}