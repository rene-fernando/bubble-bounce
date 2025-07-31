using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HazardBubbleSpawner : MonoBehaviour
{
    public GameObject hazardPrefab;
    public Transform player;

    public float verticalSpacing = 10f;
    public float xRange = 2.5f;
    public float startY = 10f;
    public float spawnDelay = 0.25f;

    public float destroyBelowDistance = 10f;
    private List<GameObject> activeHazards = new List<GameObject>();

    private float lastY;

    void Start()
    {
        lastY = player.position.y + startY;
        StartCoroutine(SpawnHazardAtY(lastY));
    }

    void Update()
    {
        if (player.position.y + startY > lastY)
        {
            lastY += Random.Range(3.5f, verticalSpacing);
            StartCoroutine(SpawnHazardAtY(lastY));
        }

        for (int i = activeHazards.Count - 1; i >= 0; i--)
        {
            if (activeHazards[i] == null)
            {
                activeHazards.RemoveAt(i);
                continue;
            }

            if (activeHazards[i].transform.position.y < player.position.y - destroyBelowDistance)
            {
                Destroy(activeHazards[i]);
                activeHazards.RemoveAt(i);
            }
        }
    }

    IEnumerator SpawnHazardAtY(float baseY)
    {
        float randomX = transform.position.x + Random.Range(-xRange, xRange);
        float randomY = baseY + Random.Range(-5f, 5f); // Slightly more vertical spread

        GameObject hazard = Instantiate(hazardPrefab, new Vector3(randomX, randomY, 0f), Quaternion.identity);
        activeHazards.Add(hazard);

        Collider2D hazardCollider = hazard.GetComponent<Collider2D>();
        if (hazardCollider != null)
        {
            hazardCollider.isTrigger = true;
        }

        HazardCollision destroyScript = hazard.AddComponent<HazardCollision>();

        // Load and assign the damage sound
        AudioClip damageSound = Resources.Load<AudioClip>("sounds/damage");
        if (damageSound != null)
        {
            AudioSource audioSource = hazard.AddComponent<AudioSource>();
            audioSource.clip = damageSound;
            destroyScript.damageSound = damageSound;
            destroyScript.audioSource = audioSource;
        }
        else
        {
            Debug.LogWarning("Damage sound not found in Resources/sounds/damage");
        }

        // Randomize movement so they aren't synchronized
        HazardMover mover = hazard.GetComponent<HazardMover>();
        if (mover != null)
        {
            mover.moveSpeed += Random.Range(-0.5f, 0.5f);
            mover.moveDistance += Random.Range(-1f, 1f);
            mover.startDelay = Random.Range(0f, 1f);
        }

        yield return new WaitForSeconds(spawnDelay);
    }
}