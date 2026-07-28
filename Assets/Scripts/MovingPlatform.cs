using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float moveHeight = 8f;
    [SerializeField] private float moveSpeed = 2f;

    private float bottomY;
    private float topY;
    private int direction = 1;

    void Awake()
    {
        bottomY = transform.position.y;
        topY = bottomY + moveHeight;
    }

    void Update()
    {
        transform.position += Vector3.up * direction * moveSpeed * Time.deltaTime;

        if (transform.position.y >= topY)
        {
            direction = -1;
        }
        else if (transform.position.y <= bottomY)
        {
            direction = 1;
        }
    }
}
