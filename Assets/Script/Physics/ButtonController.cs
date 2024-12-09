using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private List<GameObject> controlledPlatforms; // List MovingPlatform
    [SerializeField] private List<GameObject> controlledGates;     // List WallGate

    private bool isPressed = false; // Status tombol

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("InteractAble"))
        {
            isPressed = true;
            UpdateControlledObjects();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("InteractAble"))
        {
            isPressed = false;
            UpdateControlledObjects();
        }
    }

    private void UpdateControlledObjects()
    {
        // Perbarui semua MovingPlatform dalam daftar
        foreach (GameObject obj in controlledPlatforms)
        {
            if (obj != null)
            {
                MovingPlatform platform = obj.GetComponent<MovingPlatform>();
                if (platform != null)
                {
                    platform.SetButtonPressed(isPressed);
                }
            }
        }

        // Perbarui semua WallGate dalam daftar
        foreach (GameObject obj in controlledGates)
        {
            if (obj != null)
            {
                WallMovement gate = obj.GetComponent<WallMovement>();
                if (gate != null)
                {
                    gate.SetButtonPressed(isPressed);
                }
            }
        }
    }
}



// using System.Collections.Generic;
// using UnityEngine;

// public class ButtonController : MonoBehaviour
// {
//     // Daftar objek yang dapat berupa MovingPlatform atau WallGate
//     [SerializeField] private List<GameObject> controlledObjects;

//     private bool isPressed = false; // Status tombol

//     private void OnTriggerEnter2D(Collider2D collision)
//     {
//         if (collision.CompareTag("Player") || collision.CompareTag("InteractAble"))
//         {
//             isPressed = true;
//             UpdateControlledObjects();
//         }
//     }

//     private void OnTriggerExit2D(Collider2D collision)
//     {
//         if (collision.CompareTag("Player") || collision.CompareTag("InteractAble"))
//         {
//             isPressed = false;
//             UpdateControlledObjects();
//         }
//     }

//     private void UpdateControlledObjects()
//     {
//         foreach (GameObject obj in controlledObjects)
//         {
//             if (obj != null)
//             {
//                 // Cek apakah objek adalah MovingPlatform
//                 MovingPlatform platform = obj.GetComponent<MovingPlatform>();
//                 if (platform != null)
//                 {
//                     platform.SetButtonPressed(isPressed);
//                     continue;
//                 }

//                 // Cek apakah objek adalah WallGate
//                 WallMovement gate = obj.GetComponent<WallMovement>();
//                 if (gate != null)
//                 {
//                     gate.SetButtonPressed(isPressed);
//                 }
//             }
//         }
//     }
// }


// // using System.Collections;
// // using System.Collections.Generic;
// // using UnityEngine;

// // public class ButtonController : MonoBehaviour
// // {
// //     // Daftar platform yang dikontrol tombol ini
// //     [SerializeField] private List<MovingPlatform> controlledPlatforms;

// //     private bool isPressed = false; // Status tombol

// //     // Ketika tombol ditekan oleh Player atau Box
// //     private void OnTriggerEnter2D(Collider2D collision)
// //     {
// //         if (collision.CompareTag("Player") || collision.CompareTag("InteractAble"))
// //         {
// //             isPressed = true;  // Tombol ditekan
// //             UpdatePlatforms(); // Update status pada semua platform yang dikontrol
// //         }
// //     }

// //     // Ketika tombol dilepaskan
// //     private void OnTriggerExit2D(Collider2D collision)
// //     {
// //         if (collision.CompareTag("Player") || collision.CompareTag("InteractAble"))
// //         {
// //             isPressed = false;  // Tombol dilepas
// //             UpdatePlatforms();  // Update status pada semua platform yang dikontrol
// //         }
// //     }

// //     // Update status platform berdasarkan status tombol
// //     private void UpdatePlatforms()
// //     {
// //         foreach (MovingPlatform platform in controlledPlatforms)
// //         {
// //             if (platform != null)
// //             {
// //                 platform.SetButtonPressed(isPressed);  // Mengubah status isButtonPressed pada platform
// //             }
// //         }
// //     }

// //     // Menambahkan platform untuk dikontrol oleh tombol ini
// //     public void AddPlatform(MovingPlatform platform)
// //     {
// //         if (!controlledPlatforms.Contains(platform))
// //         {
// //             controlledPlatforms.Add(platform);
// //         }
// //     }

// //     // Menghapus platform dari kontrol tombol ini
// //     public void RemovePlatform(MovingPlatform platform)
// //     {
// //         if (controlledPlatforms.Contains(platform))
// //         {
// //             controlledPlatforms.Remove(platform);
// //         }
// //     }
// // }