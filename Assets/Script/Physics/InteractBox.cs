using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBox : MonoBehaviour
{
    public bool inSpirit;
    public bool isReal;
    public int idBox;
    public bool beingPushed;
    [SerializeField] private bool isPlayerNearby;
    public bool isMerged;
    private Rigidbody2D rb;
    private Vector2 initialPosition;
    [SerializeField] private GameObject font;
    [SerializeField] private Collider2D topDetector; 
    private Transform mergedBox;
    private List<Transform> stackedBoxes = new List<Transform>();
    private Vector2 mergedBoxInitialPosition;
    [Header("BoxCast Settings")]
    [SerializeField] private Vector2 boxSize = new Vector2(1f, 1f); // Ukuran Raycast kotak
    [SerializeField] private Vector2 boxOffset = new Vector2(0f, 0.5f); // Posisi relatif dari kotak
    [SerializeField] private LayerMask playerLayer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        beingPushed = false;
        isPlayerNearby = false;
        isMerged = false;
        initialPosition = transform.position;
        transform.position = initialPosition;
        font.SetActive(false);

        if (topDetector != null)
        {
            topDetector.isTrigger = true; 
        }
    }

    void FixedUpdate()
    {
        CheckPlayerInBox();

        if (!beingPushed && isPlayerNearby && !isMerged)
        {
            font.SetActive(true);
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else if (!beingPushed)
        {
            font.SetActive(false);
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else if (beingPushed)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            font.SetActive(false);

            if (isMerged && mergedBox != null)
            {
                mergedBox.position = new Vector2(transform.position.x, mergedBox.position.y);
            }
        }

        if (!beingPushed && isMerged && mergedBox != null)
        {
            mergedBox.position = new Vector2(transform.position.x, mergedBox.position.y);
        }

        if (mergedBox != null && !isMerged)
        {
            mergedBox.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        }
        else if (mergedBox != null && isMerged)
        {
            mergedBox.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }
    private void CheckPlayerInBox()
    {
        Vector2 boxPosition = (Vector2)transform.position + boxOffset;

        Collider2D hit = Physics2D.OverlapBox(boxPosition, boxSize, 0f, playerLayer);

        if (hit != null && hit.CompareTag("Player"))
        {
            isMerged = false;
        }
        else
        {
            isMerged = true;   
        }

        Debug.DrawLine(boxPosition - boxSize / 2, boxPosition + boxSize / 2, Color.green);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (topDetector != null && other.CompareTag("InteractAble"))
        {
            isMerged = true;
            mergedBox = other.transform; 
            mergedBoxInitialPosition = mergedBox.position; 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (topDetector != null && other.CompareTag("InteractAble"))
        {
            isMerged = false;
            mergedBox = null; 
        }
    }
}
