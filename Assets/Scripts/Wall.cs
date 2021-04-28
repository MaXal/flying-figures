using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position = new Vector2(transform.position.x - Time.smoothDeltaTime * moveSpeed, 0);
        if (transform.position.x < -10) Destroy(gameObject);
    }
}