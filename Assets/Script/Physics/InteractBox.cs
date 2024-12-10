using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InteractBox : MonoBehaviour
{
    public bool inSpirit;
    public bool isReal;
    public int idBox;
    public bool beingPushed;
    [SerializeField] private bool isPlayerNearby;
    [SerializeField] private bool isMerged; 
    private Rigidbody2D rb;
    private Vector2 initialPosition;
    [SerializeField] private GameObject font;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        beingPushed = false;
        isPlayerNearby = false;
        isMerged = false;
        initialPosition = transform.position;
        transform.position = initialPosition;
        font.SetActive(false);
    }

    void FixedUpdate()
    {
        if (!beingPushed && isPlayerNearby && !isMerged)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else if (!beingPushed && isPlayerNearby && isMerged)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        }
        else if (!beingPushed)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        }
        else if (beingPushed || isMerged)
        {
            rb.constraints = RigidbodyConstraints2D.None;
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerNearby = true;
            font.SetActive(true);

        }

        if (collision.gameObject.CompareTag("InteractAble"))
        {
            isMerged = true;
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerNearby = false;
            font.SetActive(false);
        }

        if (collision.gameObject.CompareTag("InteractAble"))
        {
            isMerged = false;
            collision.transform.SetParent(null);

        }
    }


    public void HandleMergedPush(bool beingPushed)
    {
        if (isMerged && beingPushed)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else if (!beingPushed)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
