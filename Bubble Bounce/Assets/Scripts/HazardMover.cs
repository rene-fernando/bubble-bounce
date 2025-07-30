using UnityEngine;

public class HazardMover : MonoBehaviour
{
    public float moveDistance = 4f;
    public float moveSpeed = 1f;
    public float startDelay = 0f; // delay before movement starts

    private Vector3 startPosition;
    private float direction = 1f;
    private float timer;

    void Start()
    {
        startPosition = transform.position;
        timer = startDelay; // wait before starting
    }

    void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            return;
        }

        transform.Translate(Vector3.right * direction * moveSpeed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - startPosition.x) >= moveDistance)
        {
            direction *= -1f;
        }
    }
}