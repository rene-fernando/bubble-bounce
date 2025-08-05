using UnityEngine;

public class Hazard : MonoBehaviour
{
    public GameObject popEffect; // Assign in inspector

    private HealthManager healthManager;

    void Start()
    {
        healthManager = FindFirstObjectByType<HealthManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            healthManager.TakeDamage();

            if (popEffect != null)
                Instantiate(popEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}