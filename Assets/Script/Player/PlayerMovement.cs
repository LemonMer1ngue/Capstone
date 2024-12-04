using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using static DimensionChanger;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;

    [SerializeField] private float distance;
    [SerializeField] private LayerMask boxMask;
    [SerializeField] private GameObject Box;
    public bool isHoldingBox = false;

    [SerializeField] private Text interactText;
    public int holdingBoxID = -1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        interactText.gameObject.SetActive(false);
    }


    void Update()
    {
        Move();
        Jump();
        UpdateAnimation();
        PlayerPushBox();
        UpdateInteractText();
        StopMoveAtDialog();
    }

    

    void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        float effectiveMoveSpeed = isHoldingBox ? moveSpeed * 0.5f : moveSpeed;
        rb.velocity = new Vector2(moveInput * effectiveMoveSpeed, rb.velocity.y);

        // Update the player's direction based on input
        if (moveInput < 0) // Moving left (pressing 'A')
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (moveInput > 0) // Moving right (pressing 'D')
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (isHoldingBox && Box != null)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");

            if (horizontalInput > 0 && Box.transform.position.x > transform.position.x)
            {
                // Player di kiri, box di kanan, animasi push
                animator.SetBool("isPushing", true);
                animator.SetBool("isPulling", false);
            }
            else if (horizontalInput < 0 && Box.transform.position.x > transform.position.x)
            {
                // Player di kiri, box di kanan, animasi pull
                animator.SetBool("isPushing", false);
                animator.SetBool("isPulling", true);
            }
            else if (horizontalInput < 0 && Box.transform.position.x < transform.position.x)
            {
                // Player di kanan, box di kiri, animasi push
                animator.SetBool("isPushing", true);
                animator.SetBool("isPulling", false);
            }
            else if (horizontalInput > 0 && Box.transform.position.x < transform.position.x)
            {
                // Player di kanan, box di kiri, animasi pull
                animator.SetBool("isPushing", false);
                animator.SetBool("isPulling", true);
            }
            else
            {
                // Tidak ada input, matikan push dan pull
                animator.SetBool("isPushing", false);
                animator.SetBool("isPulling", false);
            }
        }
        else
        {
            // Tidak sedang memegang box, matikan push dan pull
            animator.SetBool("isPushing", false);
            animator.SetBool("isPulling", false);
        }
    }


    void Jump()
    {
        switch (isHoldingBox)
        {
            case true:
                return;
            case false:
                break;
        }
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void UpdateAnimation()
    {
        animator.SetBool("isWalking", Mathf.Abs(rb.velocity.x) > 0.1f);
        animator.SetBool("isGround", isGrounded);
        animator.SetBool("isJumping", !isGrounded);

        if (!isGrounded && rb.velocity.y < 0)
        {
            animator.SetTrigger("isFalling");
        }
        else if (isGrounded)
        {
            animator.ResetTrigger("isFalling"); // Reset the isFalling trigger when grounded
        }
    }

    void PlayerPushBox()
    {
        Vector2[] directions = { Vector2.right, Vector2.left, Vector2.up, Vector2.down };

        foreach (Vector2 direction in directions)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, boxMask);
            if (hit.collider != null && hit.collider.gameObject.CompareTag("InteractAble") && Input.GetKeyDown(KeyCode.F))
            {
                InteractBox boxScript = hit.collider.gameObject.GetComponent<InteractBox>();
                if (boxScript != null)
                {
                    Debug.Log($"{boxScript.idBox}");
                }
                switch (!isHoldingBox)
                {
                    case true:
                        Box = hit.collider.gameObject;
                        Box.GetComponent<FixedJoint2D>().enabled = true;
                        Box.GetComponent<InteractBox>().beingPushed = true;
                        Box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
                        isHoldingBox = true;
                        holdingBoxID = boxScript.idBox;
                        boxScript.HandleMergedPush(true);
                        break;

                    case false:
                        Box.GetComponent<FixedJoint2D>().enabled = false;

                        Box.GetComponent<InteractBox>().beingPushed = false;
                        Box.GetComponent<FixedJoint2D>().connectedBody = null;
                        isHoldingBox = false;
                        boxScript.HandleMergedPush(false);
                        holdingBoxID = -1;

                        break;
                }
                break; 
            }
        }
    }

    void UpdateInteractText()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, distance, boxMask);

        bool showInteract = false;

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.CompareTag("InteractAble") && !isHoldingBox)
            {
                showInteract = true;
                break;
            }
        }

        interactText.gameObject.SetActive(showInteract);
    }
    void CheckBoxDrop()
    {
        switch(isHoldingBox && (!IsGrounded() || !IsBoxGrounded(Box))){
            case true:
                Debug.Log("Player or box is not grounded. Releasing box.");
                Box.GetComponent<FixedJoint2D>().enabled = false;
                Box.GetComponent<InteractBox>().beingPushed = false;
                Box.GetComponent<FixedJoint2D>().connectedBody = null;
                Box = null;
                isHoldingBox = false;
                holdingBoxID = -1;
                break;
                case false:
                break;
        }
    }
    void StopMoveAtDialog()
    {
        if (DialogueManager.Instance != null)
        {
            rb.velocity = Vector2.zero;
            return;
        }
            
        
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    bool IsBoxGrounded(GameObject box)
    {
        return Physics2D.OverlapCircle(box.transform.position, 0.1f, groundLayer);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * distance);
    }
}
