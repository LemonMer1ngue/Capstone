using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.VFX;
using static DimensionChanger;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask movingPlatformLayer;
    [SerializeField] private float raycastDistance;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private BoxCollider2D boxcollider;

    [Header("Interact To Box")]
    [SerializeField] private float distance;
    [SerializeField] private LayerMask boxMask;
    [SerializeField] private GameObject Box;
    public bool isHoldingBox = false;
    public int holdingBoxID = -1;
    public ParticleSystem dust;
    [SerializeField] private float maxDistanceToBox = 1f;
    private bool isMoving = false;

    [Header("Physics Materials")]
    [SerializeField] private PhysicsMaterial2D highFrictionMaterial;
    [SerializeField] private PhysicsMaterial2D lowFrictionMaterial;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxcollider = GetComponent<BoxCollider2D>();

    }

    void Update()
    {
        isMoving = Input.GetAxisRaw("Horizontal") != 0;
        Move();
        Jump();
        UpdateAnimation();
        PlayerPushBox();
        CheckBoxDrop();
        GroundCheck();
        CheckBoxDistance();
        UpdatePhysicsMaterial();
    }
    void CheckBoxDistance()
    {
        if (isHoldingBox && Box != null)
        {
            Vector2 directionToBox = Box.transform.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToBox.normalized, maxDistanceToBox, boxMask);
            Debug.DrawRay(transform.position, directionToBox.normalized * maxDistanceToBox, Color.red);

            if (hit.collider == null)
            {
                Debug.Log("Box dropped due to exceeding maximum distance or no longer in line of sight.");
                Box.GetComponent<InteractBox>().beingPushed = false;
                Box = null;
                isHoldingBox = false;
                holdingBoxID = -1;

                ResetAnimation();
                StopBox();
            }
        }
    }

    void Move()
    {

        float moveInput = Input.GetAxisRaw("Horizontal");
        if (isHoldingBox && Box != null)
        {
            animationInteractBox(moveInput);

            if (moveInput != 0)
            {
                MoveBox(moveInput);
            }
            else
            {
                StopPlayer();
                StopBox();
            }
        }
        else
        {
            MovePlayer(moveInput);
        }


    }
    public void UpdatePhysicsMaterial() { float moveInput = Input.GetAxisRaw("Horizontal"); 
        if (IsOnMovingPlatform()) { boxcollider.sharedMaterial = lowFrictionMaterial; } 
        else if (IsGrounded()) { 
            if (moveInput == 0) { boxcollider.sharedMaterial = highFrictionMaterial; } 
            else { boxcollider.sharedMaterial = lowFrictionMaterial; } } }
    bool IsOnMovingPlatform() { 
        RaycastHit2D hit = Physics2D.Raycast(boxcollider.bounds.center, Vector2.down, boxcollider.bounds.extents.y + raycastDistance, movingPlatformLayer); 
        return hit.collider != null; }
    void MovePlayer(float moveInput)
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        if (isHoldingBox)
        {
            animationInteractBox(moveInput);
        }
        else
        {
            if (moveInput != 0 && (isGrounded == true))
            {
                transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
                dust.Play();
            }

        }

    }
    void MoveBox(float moveInput)
    {
        float adjustedSpeed = isHoldingBox ? moveSpeed / 2 : moveSpeed;

        rb.velocity = new Vector2(moveInput * adjustedSpeed, rb.velocity.y);
        Rigidbody2D boxRb = Box.GetComponent<Rigidbody2D>();
        boxRb.velocity = new Vector2(moveInput * adjustedSpeed, boxRb.velocity.y);


    }

    void StopPlayer()
    {
        rb.velocity = Vector2.zero;
    }

    void StopBox()
    {
        if (Box != null)
        {
            Rigidbody2D boxRb = Box.GetComponent<Rigidbody2D>();
            boxRb.velocity = Vector2.zero;
        }

    }

    private void animationInteractBox(float moveInput)
    {
        if (moveInput > 0 && Box.transform.position.x > transform.position.x)
        {
            animator.SetBool("isPushing", true);
            animator.SetBool("isPulling", false);
        }
        else if (moveInput < 0 && Box.transform.position.x > transform.position.x)
        {
            animator.SetBool("isPushing", false);
            animator.SetBool("isPulling", true);
        }
        else if (moveInput > 0 && Box.transform.position.x < transform.position.x)
        {
            animator.SetBool("isPushing", false);
            animator.SetBool("isPulling", true);
        }
        else if (moveInput < 0 && Box.transform.position.x < transform.position.x)
        {
            animator.SetBool("isPushing", true);
            animator.SetBool("isPulling", false);
        }
        else
        {
            animator.SetBool("isPushing", false);
            animator.SetBool("isPulling", false);
        }


    }
    void Jump()
    {
        if (!isHoldingBox && IsGrounded() && Input.GetButtonDown("Jump"))
        {
            dust.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Menerapkan gaya lompat
        }

    }
    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(boxcollider.bounds.center, Vector2.down, boxcollider.bounds.extents.y + raycastDistance, groundLayer);
        Debug.DrawRay(boxcollider.bounds.center, Vector2.down * (boxcollider.bounds.extents.y + raycastDistance), Color.red); // Debugging
        return hit.collider != null;
    }

    void GroundCheck()
    {
        if (IsGrounded() == true)
        {
            animator.SetBool("isGround", true);
            isGrounded = true;
        }
        else
        {
            animator.SetBool("isGround", false);
            isGrounded = false;
        }
    }

    void UpdateAnimation()
    {
        animator.SetBool("isWalking", Input.GetAxisRaw("Horizontal") != 0 && !isHoldingBox);

        if (!isGrounded && rb.velocity.y < 0)
        {
            animator.SetTrigger("isFalling");
        }
        else if (isGrounded)
        {
            animator.ResetTrigger("isFalling"); // Reset the isFalling trigger when grounded
        }
    }
    void ResetAnimation()
    {
        animator.SetBool("isPulling", false);
        animator.SetBool("isPushing", false);
        animator.SetBool("isWalking", Input.GetAxisRaw("Horizontal") != 0);
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

                if (!isHoldingBox)
                {
                    Box = hit.collider.gameObject;
                    Box.GetComponent<InteractBox>().beingPushed = true;
                    isHoldingBox = true;
                    holdingBoxID = boxScript.idBox;
                }
                else
                {
                    isHoldingBox = false;
                    Box.GetComponent<InteractBox>().beingPushed = false;
                    holdingBoxID = -1;
                    ResetAnimation();
                    StopBox();
                }

            }
        }
    }


    void CheckBoxDrop()
    {
        if (isHoldingBox && Box != null)
        {
            if (!IsBoxGrounded(Box))
            {
                Box.GetComponent<InteractBox>().beingPushed = false;
                Box = null;
                isHoldingBox = false;
                holdingBoxID = -1;

                ResetAnimation();
                StopBox();
            }
        }
    }
    bool IsBoxGrounded(GameObject box)
    {
        RaycastHit2D hit = Physics2D.Raycast(box.transform.position, Vector2.down, raycastDistance, groundLayer);
        Debug.DrawRay(box.transform.position, Vector2.down * raycastDistance, Color.blue);
        return hit.collider != null;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * distance);
    }

    internal bool IsMoving()
    {
        return isMoving || rb.velocity.x != 0 || rb.velocity.y != 0;
    }
}
