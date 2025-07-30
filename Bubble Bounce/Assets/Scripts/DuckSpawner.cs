using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DuckSpawner : MonoBehaviour
{
    public GameObject duckPrefab;
    public int numberOfDucks = 10;
    public float verticalOffset = 0.5f;
    public float minDistanceBetweenDucks = 0.75f;
    public Transform player; // Assign this in the inspector
    public float duckSpawnInterval = 2f;

    void Start()
    {
        StartCoroutine(SpawnDucksContinuously());
    }

    private IEnumerator SpawnDucksContinuously()
    {
        while (true)
        {
            SpawnDuckWave();
            yield return new WaitForSeconds(duckSpawnInterval);
        }
    }

    private void SpawnDuckWave()
    {
        GameObject[] bubbles = GameObject.FindGameObjectsWithTag("Platform");

        if (bubbles.Length == 0)
        {
            Debug.LogWarning("No bubbles found to spawn ducks on.");
            return;
        }

        List<Transform> chosenSpots = new List<Transform>();
        int attempts = 0;
        int maxAttempts = 200;

        while (chosenSpots.Count < numberOfDucks && attempts < maxAttempts)
        {
            GameObject bubble = bubbles[Random.Range(0, bubbles.Length)];
            Vector3 pos = bubble.transform.position;

            // Favor bubbles around or above the player
            if (!bubble.activeInHierarchy ||
                pos.x < -7f || pos.x > 7f ||
                pos.y < player.position.y || pos.y > player.position.y + 30f)
            {
                attempts++;
                continue;
            }

            bool tooClose = chosenSpots.Any(chosen => Vector3.Distance(chosen.position, pos) < minDistanceBetweenDucks);
            if (!tooClose || attempts > maxAttempts / 4)
            {
                Vector3 duckPos = pos + new Vector3(0, verticalOffset, 0);
                GameObject duck = Instantiate(duckPrefab, duckPos, Quaternion.identity);
                StartCoroutine(ParentDuckNextFrame(duck.transform, bubble.transform));
                chosenSpots.Add(bubble.transform);
            }

            attempts++;
        }

        if (attempts >= maxAttempts)
        {
            Debug.LogWarning("Stopped duck placement early due to too many overlap attempts.");
        }
    }

    private IEnumerator ParentDuckNextFrame(Transform duck, Transform bubble)
    {
        yield return new WaitForEndOfFrame();

        if (duck != null && bubble != null && bubble.gameObject.activeInHierarchy)
        {
            duck.SetParent(bubble);
        }
    }
}