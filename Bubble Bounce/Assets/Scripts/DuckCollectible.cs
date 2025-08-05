using UnityEngine;
using UnityEngine.Audio;

public class DuckCollectible : MonoBehaviour
{
    public AudioClip collectSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        // Route to SFX AudioMixerGroup if available
        AudioMixerGroup[] mixerGroups = Resources.FindObjectsOfTypeAll<AudioMixerGroup>();
        foreach (var group in mixerGroups)
        {
            if (group.name == "SFX") // Make sure this matches your AudioMixer group name
            {
                audioSource.outputAudioMixerGroup = group;
                break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FindFirstObjectByType<ScoreManager>().AddScore(2);

            if (collectSound != null)
            {
                audioSource.PlayOneShot(collectSound);
            }

            Destroy(gameObject);
        }
    }
}