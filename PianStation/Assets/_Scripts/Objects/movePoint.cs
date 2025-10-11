using UnityEngine;

public class movePoint : MonoBehaviour
{
    public float speed = 1.5f;
    public float height = 0.05f;

    private Vector3 startPosition;
    void Start()
    {
        startPosition = transform.localPosition;
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * speed) * height;
        transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
