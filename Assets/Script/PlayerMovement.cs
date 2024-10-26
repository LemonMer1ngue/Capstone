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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    void Update()
    {
        Move(); // Handle movement
        Jump(); // Handle jumping
        UpdateAnimation(); // Update animations based on state
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
}
