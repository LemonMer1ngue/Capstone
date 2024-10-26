using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player
    public float jumpForce = 10f; // Force applied when jumping
    public Transform groundCheck; // Position to check if the player is grounded
    public LayerMask groundLayer; // LayerMask to identify ground

    private Rigidbody2D rb; // Reference to the player's Rigidbody2D
    private Animator animator; // Reference to the player's Animator
    private bool isGrounded; // Track whether the player is grounded

    public float distance;
    public LayerMask boxMask;
    GameObject Box;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    void Update()
    {
        Move();
        Jump();
        UpdateAnimation();
        PlayerPushBox();
    }

    void Move()
    {
        if (PauseMenu.IsPaused) return; // Check if the game is paused

        float moveInput = Input.GetAxisRaw("Horizontal"); // Get horizontal input
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y); // Set velocity

        // Flip the sprite based on horizontal input
        if (moveInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
        }
    }

    void Jump()
    {
        if (PauseMenu.IsPaused) return; // Check if the game is paused

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer); // Check if grounded
        if (isGrounded && Input.GetButtonDown("Jump")) // Jump if grounded
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Apply jump force
        }
    }

    void UpdateAnimation()
    {
        animator.SetBool("isWalking", Mathf.Abs(rb.velocity.x) > 0.1f); // Update walking animation
        animator.SetBool("isGround", isGrounded); // Update grounded state
        animator.SetBool("isJumping", !isGrounded); // Update jumping state

        // Trigger falling animation
        if (!isGrounded && rb.velocity.y < 0)
        {
            animator.SetTrigger("isFalling");
        }
        else if (isGrounded)
        {
            animator.ResetTrigger("isFalling"); // Reset falling trigger when grounded
        }
    }

    void PlayerPushBox()
    {
        RaycastHit2D hit =  Physics2D.Raycast(transform.position, Vector2.right* transform.localScale, distance, boxMask);
        if (hit.collider != null) 
        {
            switch (Input.GetKeyDown(KeyCode.F))
            {
                case true: 
                    Box = hit.collider.gameObject;
                    Box.GetComponent<FixedJoint2D>().enabled = true;
                    Box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();

                    break;
                default:
                    break;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * distance);
    }

}
