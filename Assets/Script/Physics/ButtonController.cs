using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    // Daftar platform yang dikontrol tombol ini
    [SerializeField] private List<MovingPlatform> controlledPlatforms;

    private bool isPressed = false; // Status tombol

    // Ketika tombol ditekan oleh Player atau Box
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("InteractAble"))
        {
            isPressed = true;  // Tombol ditekan
            UpdatePlatforms(); // Update status pada semua platform yang dikontrol
        }
    }

    // Ketika tombol dilepaskan
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("InteractAble"))
        {
            isPressed = false;  // Tombol dilepas
            UpdatePlatforms();  // Update status pada semua platform yang dikontrol
        }
    }

    // Update status platform berdasarkan status tombol
    private void UpdatePlatforms()
    {
        foreach (MovingPlatform platform in controlledPlatforms)
        {
            if (platform != null)
            {
                platform.SetButtonPressed(isPressed);  // Mengubah status isButtonPressed pada platform
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
}