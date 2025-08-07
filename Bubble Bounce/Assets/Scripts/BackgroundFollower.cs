using UnityEngine;

public class BackgroundFollower : MonoBehaviour
{
    public Transform target; 
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        transform.position = new Vector3(
            transform.position.x,
            target.position.y + offset.y,
            transform.position.z
        );
    }
}