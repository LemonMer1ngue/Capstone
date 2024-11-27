using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxStackController : MonoBehaviour
{
    public bool isBottomBox; // Tandai apakah ini box bawah
    public Rigidbody2D boxRb; // Rigidbody dari box ini
    private Rigidbody2D topBoxRb; // Rigidbody dari box atas
    private bool isCollidingWithTopBox = false;

    void Start()
    {
        if (boxRb == null)
            boxRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isBottomBox && !GetComponent<InteractBox>().beingPushed && isCollidingWithTopBox)
        {
            // Jika box bawah tidak didorong dan bersentuhan dengan box atas, freeze posisi X box atas
            if (topBoxRb != null)
            {
                topBoxRb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
            }
        }
        else if (topBoxRb != null)
        {
            // Jika box bawah didorong atau tidak bersentuhan, lepaskan freeze posisi X box atas
            topBoxRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("InteractAble"))
        {
            Rigidbody2D collidedRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (collidedRb != null && isBottomBox)
            {
                topBoxRb = collidedRb;
                isCollidingWithTopBox = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("InteractAble"))
        {
            Rigidbody2D collidedRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (collidedRb != null && topBoxRb == collidedRb)
            {
                topBoxRb = null;
                isCollidingWithTopBox = false;
            }
        }
    }
}
