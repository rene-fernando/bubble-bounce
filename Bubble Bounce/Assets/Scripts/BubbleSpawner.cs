using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab;
    public int numberOfBubbles = 30;
    public float verticalSpacing = 2.5f;
    public float xRange = 2.5f;
    public float startY = -3f;

    public static List<Vector3> bubblePositions = new List<Vector3>();

    private float lastX = 0f;

    void Start()
    {
        for (int i = 0; i < numberOfBubbles; i++)
        {
            float y = startY + i * verticalSpacing;

            float x;
            do
            {
                x = Random.Range(-xRange, xRange);
            } while (Mathf.Abs(x - lastX) < 1f);

            lastX = x;

            Vector3 spawnPos = new Vector3(x, y, 0);
            Instantiate(bubblePrefab, spawnPos, Quaternion.identity);
            bubblePositions.Add(spawnPos); // Save this position
        }
    }
}