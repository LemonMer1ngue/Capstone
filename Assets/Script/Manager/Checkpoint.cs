using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    DeathController DeathController;
    Collider2D Collider;
    Animator animator;

    private void Awake()
    {
        DeathController = FindObjectOfType<DeathController>();
        Collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        if (DeathController == null)
        {
            Debug.LogError("DeathController tidak ditemukan di scene!");
        }
   
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
