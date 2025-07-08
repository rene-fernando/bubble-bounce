using UnityEngine;

public class DuckCollectible : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FindFirstObjectByType<ScoreManager>().AddScore(1);
            Destroy(gameObject);
        }
    }
}