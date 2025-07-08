using UnityEngine;

public class PlayerBounce : MonoBehaviour
{
    [Range(1f, 20f)]
    public float bounceForce = 8f;
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private bool canBounce = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);

        if (canBounce && Input.GetKeyDown(KeyCode.Space))
        {
            Bounce();
        }

        // Game over if player falls below camera view
        if (transform.position.y < Camera.main.transform.position.y - 10f)
        {
            Debug.Log("You fell! Game Over.");
            // You can reload the scene or trigger a UI
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }

    private void Bounce()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
        canBounce = false; // prevent double bounce

        Debug.Log("Bounced with force: " + bounceForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            canBounce = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            canBounce = false;
        }
    }
}