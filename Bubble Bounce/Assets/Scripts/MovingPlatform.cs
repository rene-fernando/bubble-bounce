using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveDistance = 2f;
    public float moveSpeed = 1f;
    public float bobAmplitude = 0.2f;
    public float bobFrequency = 1f;

    private Vector3 startPos;
    private int direction = 1;
    private float timeOffset;

    void Start()
    {
        startPos = transform.position;
        timeOffset = Random.Range(0f, 2f * Mathf.PI); // prevents synchronized bobbing
    }

    void Update()
    {
        float horizontalMovement = direction * moveSpeed * Time.deltaTime;
        float verticalOffset = Mathf.Sin(Time.time * bobFrequency + timeOffset) * bobAmplitude;
        transform.position = new Vector3(
            transform.position.x + horizontalMovement,
            startPos.y + verticalOffset,
            transform.position.z
        );

        if (Mathf.Abs(transform.position.x - startPos.x) >= moveDistance)
        {
            direction *= -1;
        }
    }
}