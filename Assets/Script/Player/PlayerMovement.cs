using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using static DimensionChanger;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;

    public float distance;
    public LayerMask boxMask;
    public GameObject Box;
    public bool isHoldingBox = false;

    [SerializeField]
    private Text interactText;


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

    public void UpdateBoxReference(GameObject newBox)
    {
        Box = newBox;
        Box.GetComponent<FixedJoint2D>().enabled = true;
        Box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
        isHoldingBox = true;
    }

    void PlayerPushBox()
    {
        Vector2[] directions = { Vector2.right, Vector2.left, Vector2.up, Vector2.down };

        foreach (Vector2 direction in directions)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, boxMask);
            if (hit.collider != null && hit.collider.gameObject.CompareTag("InteractAble") && Input.GetKeyDown(KeyCode.F))
            {
                switch (isHoldingBox)
                {
                    case false:
                        Box = hit.collider.gameObject;
                        Box.GetComponent<FixedJoint2D>().enabled = true;
                        Box.GetComponent<InteractBox>().beingPushed = true;
                        Box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
                        isHoldingBox = true;
                        break;

                    case true:
                        Box.GetComponent<FixedJoint2D>().enabled = false;
                        Box.GetComponent<InteractBox>().beingPushed = false;
                        Box.GetComponent<FixedJoint2D>().connectedBody = null;
                        Box = null;
                        isHoldingBox = false;
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * distance);
    }
}
