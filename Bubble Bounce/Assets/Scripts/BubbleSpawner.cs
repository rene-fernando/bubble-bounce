using UnityEngine;
using System.Collections.Generic;

public class BubbleSpawner : MonoBehaviour
{
    public float bobbingAmplitude = 0.1f;
    public float bobbingFrequency = 1f;

    public GameObject bubblePrefab;
    public int numberOfInitialBubbles = 30;
    public float verticalSpacing = 2.5f;
    public float xRange = 2.5f;
    public float startY = -3f;
    [Range(0f, 1f)] public float movingBubbleChance = 0.3f;

    public Transform player;
    public float spawnBufferAbove = 15f;
    public float destroyBelowDistance = 10f;

    private float lastX = 0f;
    private float highestYSpawned;
    private List<GameObject> activeBubbles = new List<GameObject>();

    void Start()
    {
        highestYSpawned = startY;

        // Spawn bubbles initially to fill buffer above player
        while (highestYSpawned < player.position.y + spawnBufferAbove)
        {
            SpawnNextBubble();
        }
    }

    void Update()
    {
        // Spawn more bubbles ahead of the player
        while (highestYSpawned < player.position.y + spawnBufferAbove)
        {
            SpawnNextBubble();
        }

        // Destroy bubbles that are too far below
        for (int i = activeBubbles.Count - 1; i >= 0; i--)
        {
            if (activeBubbles[i].transform.position.y < player.position.y - destroyBelowDistance)
            {
                Destroy(activeBubbles[i]);
                activeBubbles.RemoveAt(i);
            }
        }
    }

    void SpawnNextBubble()
    {
        float x;
        do
        {
            x = Random.Range(-xRange, xRange);
        } while (Mathf.Abs(x - lastX) < 1f);

        lastX = x;
        Vector3 spawnPos = new Vector3(x, highestYSpawned, 0);

        GameObject bubble = Instantiate(bubblePrefab, spawnPos, Quaternion.identity);
        activeBubbles.Add(bubble);

        bool isMovingHorizontally = Random.value < movingBubbleChance;
        if (isMovingHorizontally)
        {
            bubble.AddComponent<MovingPlatform>();
        }
        // All bubbles will bob regardless of movement type
        bubble.AddComponent<BubbleBobbing>().Initialize(bobbingAmplitude, bobbingFrequency);

        highestYSpawned += verticalSpacing;
    }
}
// Bobbing behavior for all bubbles
public class BubbleBobbing : MonoBehaviour
{
    private float amplitude;
    private float frequency;
    private Vector3 startPos;

    public void Initialize(float amp, float freq)
    {
        amplitude = amp;
        frequency = freq;
        startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}