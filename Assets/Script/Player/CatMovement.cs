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

    private void Awake()
    {
        GameManager.Instance.CatMovement = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxcollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        Move();
        Jump();
        GroundCheck();
        UpdateAnimation();
    }

    void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        MovePlayer(moveInput);
    }

    void MovePlayer(float moveInput)
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput != 0 && isGrounded)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput) * 0.6f, 0.6f, 0.6f);
        }
        else if (moveInput == 0)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    void Jump()
    {
        if (IsGrounded() && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(boxcollider.bounds.center, Vector2.down, boxcollider.bounds.extents.y + raycastDistance, groundLayer);
        Debug.DrawRay(boxcollider.bounds.center, Vector2.down * (boxcollider.bounds.extents.y + raycastDistance), Color.red);
        return hit.collider != null;
    }

    void GroundCheck()
    {
        isGrounded = IsGrounded();
        animator.SetBool("isGround", isGrounded);
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
            animator.ResetTrigger("isFalling");
        }
    }


    #region Save and Load
    public void Save(ref CatSavedata data)
    {
        data.Position = transform.position;
    }

    public void Load(CatSavedata data) 
    {
        transform.position = data.Position;
    }
    #endregion
}

[System.Serializable]
public struct CatSavedata
{
    public Vector3 Position;
}
