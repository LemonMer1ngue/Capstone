using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedAnimatorCheckPoint : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Aktifkan Animator
            if (animator != null)
            {
                animator.enabled = true; // Aktifkan Animator
                Debug.Log("Animator diaktifkan (Trigger).");
            }
        }
    }
}
