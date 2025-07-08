using UnityEngine;

public class Hazard : MonoBehaviour
{
    private HealthManager healthManager;

    void Start()
    {
        healthManager = FindFirstObjectByType<HealthManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
{
    Debug.Log("Triggered with: " + other.name);

    if (other.CompareTag("Player"))
    {
        Debug.Log("Player touched hazard!");
        healthManager.TakeDamage();
    }
}
}