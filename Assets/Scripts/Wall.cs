using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private GameObject tile;

    private void Start()
    {
        for (var i = 0; i < 9; i++)
        {
            var generatedTile = Instantiate(tile, new Vector3(transform.position.x, 1 + i * 2, 0),
                Quaternion.Euler(0, 0, 90));
            generatedTile.transform.parent = gameObject.transform;
        }
    }

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