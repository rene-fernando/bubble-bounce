using UnityEngine;

public class HazardCollision : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            // Optional: Trigger damage, animation, or sound here
        }
    }
}