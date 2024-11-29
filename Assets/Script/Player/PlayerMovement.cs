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

    }

    void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        float effectiveMoveSpeed = isHoldingBox ? moveSpeed * 0.5f : moveSpeed;
        rb.velocity = new Vector2(moveInput * effectiveMoveSpeed, rb.velocity.y);

        if (moveInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
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
                    Debug.Log($"Interacted with Box ID: {boxScript.idBox}");
                }
                switch (!isHoldingBox && IsBoxGrounded(hit.collider.gameObject))
                {
                    case true:
                        Box = hit.collider.gameObject;
                        Box.GetComponent<FixedJoint2D>().enabled = true;
                        //FixedJoint2D joint = Box.GetComponent<FixedJoint2D>();
                        //Rigidbody2D rigidbody = Box.GetComponent<Rigidbody2D>();
                        //rigidbody.freezeRotation = false;
                        //joint.frequency = 3f;
                        Box.GetComponent<InteractBox>().beingPushed = true;
                        Box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
                        isHoldingBox = true;
                        holdingBoxID = boxScript.idBox;
                        break;

                    case false:
                        Box.GetComponent<FixedJoint2D>().enabled = false;
                        //FixedJoint2D releaseJoint = Box.GetComponent<FixedJoint2D>();
                        //Rigidbody2D releaserigidbody = Box.GetComponent<Rigidbody2D>();
                        //releaserigidbody.freezeRotation = true;
                        //releaseJoint.frequency = 0f;
                        Box.GetComponent<InteractBox>().beingPushed = false;
                        Box.GetComponent<FixedJoint2D>().connectedBody = null;
                        isHoldingBox = false;
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