using UnityEngine;
using UnityEngine.Audio;

public class DuckCollectible : MonoBehaviour
{
    public AudioClip collectSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        AudioMixerGroup[] mixerGroups = Resources.FindObjectsOfTypeAll<AudioMixerGroup>();
        foreach (var group in mixerGroups)
        {
            if (group.name == "SFX") 
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

            FindFirstObjectByType<DuckSpawner>()?.ClearCurrentDuck();

            Destroy(gameObject);
        }
    }
}