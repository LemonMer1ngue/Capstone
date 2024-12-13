using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Checkpoint : MonoBehaviour
{
    DeathController DeathController;
    Collider2D Collider;
    Animator animator;

    private void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            DeathController = FindObjectOfType<DeathController>();
            if (DeathController == null)
            {
                Debug.LogError("DeathController tidak ditemukan pada object player!");
            }

        }
        else
        {
            Debug.LogError("Player tidak ditemukan di scene!");
        }

        Collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        
   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (DeathController != null)
            {
                animator.enabled = true;
                DeathController.UpdateCheckpoint(transform.position);
                Collider.enabled = false;
                SaveSystem.Save();
            }
        }
    }
}
