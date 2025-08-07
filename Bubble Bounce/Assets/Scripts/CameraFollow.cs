using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;    
    public float yOffset = 2f;    
    public float smoothSpeed = 0.2f;

    private float highestY;

    void Start()
    {
        if (player != null)
            highestY = player.position.y;
    }

    void LateUpdate()
    {
        if (player == null) return;

        float desiredY = player.position.y + yOffset;

        if (desiredY > highestY)
        {
            highestY = desiredY;
            Vector3 targetPos = new Vector3(transform.position.x, highestY, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed);
        }
    }
}