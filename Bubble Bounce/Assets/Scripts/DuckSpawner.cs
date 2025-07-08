using UnityEngine;

public class DuckSpawner : MonoBehaviour
{
    public GameObject duckPrefab;
    public int numberOfDucks = 10;

    void Start()
    {
        StartCoroutine(SpawnDucksWithDelay());
    }

    System.Collections.IEnumerator SpawnDucksWithDelay()
    {
        yield return new WaitForSeconds(0.1f); // wait a frame or two

        var positions = BubbleSpawner.bubblePositions;

        if (positions.Count == 0)
        {
            Debug.LogWarning("No bubble positions found!");
            yield break;
        }

        for (int i = 0; i < numberOfDucks; i++)
        {
            Vector3 basePos = positions[Random.Range(5, positions.Count)];

            float xOffset = Random.Range(-0.5f, 0.5f);
            float yOffset = Random.Range(0.6f, 1.2f);

            Vector3 duckPos = new Vector3(basePos.x + xOffset, basePos.y + yOffset, 0);
            Instantiate(duckPrefab, duckPos, Quaternion.identity);
        }
    }
}