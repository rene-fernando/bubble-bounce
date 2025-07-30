using UnityEngine;

public class DuckMover : MonoBehaviour
{
    public float moveDistance = 1f;
    public float moveSpeed = 2f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition = startPos + Vector3.right * Mathf.Sin(Time.time * moveSpeed) * moveDistance;
    }
}