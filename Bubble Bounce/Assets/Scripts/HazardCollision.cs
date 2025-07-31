using UnityEngine;

public class HazardCollision : MonoBehaviour
{
    public AudioClip damageSound;
    public AudioSource audioSource;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (damageSound != null)
            {
                AudioSource playerAudio = other.GetComponent<AudioSource>();
                if (playerAudio != null)
                {
                    playerAudio.PlayOneShot(damageSound);
                }
            }
            Destroy(gameObject);
            // Optional: Trigger damage, animation, or sound here
        }
    }
}