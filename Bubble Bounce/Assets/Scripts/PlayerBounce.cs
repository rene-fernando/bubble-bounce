using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBounce : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    public Sprite babyRight;
    public Sprite babyLeft;
    public Sprite babyRightJump;
    public Sprite babyLeftJump;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isJumping = false;
    private GameObject lastBouncedPlatform = null;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        // Horizontal movement
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Jumping when Space is pressed
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isJumping = true;
        }

        // Landed
        if (IsGrounded() && rb.linearVelocity.y <= 0.1f)
        {
            isJumping = false;

            // Score logic: check if landed on a new platform
            Collider2D hit = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            if (hit != null && hit.gameObject != lastBouncedPlatform)
            {
                lastBouncedPlatform = hit.gameObject;
                ScoreManager.Instance.AddScore(1);
            }
        }

        // Flip sprite based on direction and jumping state
        if (moveInput > 0)
        {
            sr.sprite = isJumping ? babyRightJump : babyRight;
        }
        else if (moveInput < 0)
        {
            sr.sprite = isJumping ? babyLeftJump : babyLeft;
        }

        // Reset the scene if player falls too low
        Vector3 bottomOfCamera = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        if (transform.position.y < bottomOfCamera.y - 1f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(collision.transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(null);
        }
    }
}