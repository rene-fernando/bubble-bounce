using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public static class UnparentManager
{
    private static GameObject deferredTarget;

    public static void DeferUnparent(GameObject target)
    {
        deferredTarget = target;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (deferredTarget != null)
        {
            Debug.Log("Deferred unparenting on scene reload.");
            deferredTarget.transform.SetParent(null);
            deferredTarget = null;
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

public class PlayerBounce : MonoBehaviour
{
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    // Movement and sprite fields
    public Sprite babyLeft;
    public Sprite babyRight;
    public Sprite babyLeftJump;
    public Sprite babyRightJump;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private GameObject lastBouncedPlatform;
    private bool isJumping;
    private float moveInput;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        // Update facing direction
        if (moveInput > 0) facingRight = true;
        else if (moveInput < 0) facingRight = false;

        // Horizontal movement
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Update jump sprite while airborne
        if (!IsGrounded())
        {
            sr.sprite = facingRight ? babyRightJump : babyLeftJump;
        }

        // Jumping when Space is pressed
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;
            sr.sprite = facingRight ? babyRightJump : babyLeftJump;
        }

        // Landed
        if (IsGrounded() && rb.velocity.y <= 0.1f)
        {
            isJumping = false;
            sr.sprite = facingRight ? babyRight : babyLeft;
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
        if (collision.gameObject.CompareTag("Platform") && !collision.gameObject.CompareTag("Ground"))
        {
            // Parent the BubbleBaby to the platform when standing on it
            Debug.Log($"Parenting BubbleBaby to {collision.gameObject.name}");
            transform.SetParent(collision.transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform") && !collision.gameObject.CompareTag("Ground"))
        {
            if (gameObject.activeInHierarchy)
            {
                Debug.Log($"Unparenting BubbleBaby from {collision.gameObject.name}");
                StartCoroutine(UnparentAfterFrame());
            }
            else
            {
                Debug.LogWarning("BubbleBaby is inactive, deferring unparenting.");
                UnparentManager.DeferUnparent(gameObject);
            }
        }
    }

    private IEnumerator UnparentAfterFrame()
    {
        yield return null; // Wait for the next frame
        Debug.Log("Unparenting player from platform.");
        transform.SetParent(null);
    }
}