using UnityEngine;
using UnityEngine.UI;

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
    GameObject Box;
    private bool isHoldingBox = false;

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
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Flip the sprite based on horizontal input
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

    void PlayerPushBox()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale, distance, boxMask);
        if (hit.collider != null && hit.collider.gameObject.CompareTag("InteractAble") && Input.GetKeyDown(KeyCode.F))
        {
            switch (isHoldingBox)
            {
                case false:
                    Box = hit.collider.gameObject;
                    Box.GetComponent<FixedJoint2D>().enabled = true;
                    Box.GetComponent<InteractBox>().beingPushed = true;
                    Box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
                    isHoldingBox = true; break;

                case true:
                    Box.GetComponent<FixedJoint2D>().enabled = false;
                    Box.GetComponent<InteractBox>().beingPushed = false;
                    Box.GetComponent<FixedJoint2D>().connectedBody = null;
                    Box = null;
                    isHoldingBox = false; break;

            }
        }
    }
    void UpdateInteractText()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale, distance, boxMask);
        if (hit.collider != null && hit.collider.gameObject.CompareTag("InteractAble") && !isHoldingBox)
        {
            interactText.gameObject.SetActive(true);

        }
        else
        {
            interactText.gameObject.SetActive(false);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * distance);
    }

}
