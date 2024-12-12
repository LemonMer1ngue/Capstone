using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.VFX;
using static DimensionChanger;

public class CatMovement : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float raycastDistance;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private BoxCollider2D boxcollider;

    // public ParticleSystem dust;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxcollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }
    void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        MovePlayer(moveInput);
    }
    void MovePlayer(float moveInput)
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        if (moveInput != 0 && (isGrounded == true))
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
            // dust.Play();
        }
    }
    void Jump()
    {
        if (IsGrounded() && Input.GetButtonDown("Jump"))
        {
            // dust.Play();
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
        animator.SetBool("isWalking", Input.GetAxisRaw("Horizontal") != 0);

        if (!isGrounded && rb.velocity.y < 0)
        {
            animator.SetTrigger("isFalling");
        }
        else if (isGrounded)
        {
            animator.ResetTrigger("isFalling"); // Reset the isFalling trigger when grounded
        }
    }
}
