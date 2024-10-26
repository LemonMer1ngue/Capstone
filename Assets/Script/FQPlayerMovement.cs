using UnityEngine;

public class FQPlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;

    public float distance;
    public LayerMask boxMask;
    GameObject Box;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        // Jika karakter meninggalkan objek dengan tag "ground"
        if (collision.gameObject.tag == "ground")
        {
            isGrounded = false;
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
