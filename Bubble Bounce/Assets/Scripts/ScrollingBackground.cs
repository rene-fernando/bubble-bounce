using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float scrollSpeed = 0.1f;
    public float smoothing = 5f;

    private Renderer rend;
    private float currentYOffset = 0f;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float targetYOffset = Camera.main.transform.position.y * scrollSpeed;

        currentYOffset = Mathf.Lerp(currentYOffset, targetYOffset, Time.deltaTime * smoothing);

        rend.material.mainTextureOffset = new Vector2(0, currentYOffset);
    }

    void LateUpdate()
    {
        float targetYOffset = Camera.main.transform.position.y * scrollSpeed;
        currentYOffset = Mathf.Lerp(currentYOffset, targetYOffset, Time.deltaTime * smoothing);
        rend.material.mainTextureOffset = new Vector2(0, currentYOffset);
    }
}