using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio; 
using System.Collections;
using System.Collections.Generic;

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

    public Sprite babyLeft;
    public Sprite babyRight;
    public Sprite babyLeftJump;
    public Sprite babyRightJump;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public AudioClip bounceSound;
    public AudioMixerGroup sfxMixerGroup; 

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private HashSet<GameObject> visitedPlatforms = new HashSet<GameObject>();
    private bool isJumping;
    private float moveInput;
    private bool facingRight = true;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        if (sfxMixerGroup != null)
        {
            audioSource.outputAudioMixerGroup = sfxMixerGroup; 
        }
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput > 0) facingRight = true;
        else if (moveInput < 0) facingRight = false;

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (!IsGrounded())
        {
            sr.sprite = facingRight ? babyRightJump : babyLeftJump;
        }

        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            audioSource.PlayOneShot(bounceSound);
            isJumping = true;
            sr.sprite = facingRight ? babyRightJump : babyLeftJump;
        }

        if (IsGrounded() && rb.linearVelocity.y <= 0.1f)
        {
            isJumping = false;
            Collider2D hit = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            if (hit != null && hit.CompareTag("Platform") && !hit.CompareTag("Ground"))
            {
                if (!visitedPlatforms.Contains(hit.gameObject))
                {
                    visitedPlatforms.Add(hit.gameObject);
                    ScoreManager.Instance.AddScore(1);
                    Debug.Log("Score +1");
                }
            }
            sr.sprite = facingRight ? babyRight : babyLeft;
        }

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
        yield return null; 
        Debug.Log("Unparenting player from platform.");
        transform.SetParent(null);
    }
}