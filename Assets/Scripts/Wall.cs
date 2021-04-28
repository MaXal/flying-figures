using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Wall : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private GameObject tile;
    [SerializeField] private GameObject passage;

    private void Start()
    {
        CreateTiles();
    }

    private void Update()
    {
        Move();
    }

    public event Action OnWallDestroy;

    private void CreateTiles()
    {
        for (var i = 0; i < 9; i++)
        {
            var typeOfTile = Random.Range(0, 2) == 0 ? passage : tile;
            var generatedTile = Instantiate(typeOfTile, new Vector3(transform.position.x, 1 + i * 2, 0),
                Quaternion.Euler(0, 0, 90));
            generatedTile.transform.parent = gameObject.transform;
        }
    }

    private void Move()
    {
        transform.position = new Vector2(transform.position.x - Time.smoothDeltaTime * moveSpeed, 0);
        if (transform.position.x < -1) WallDestroyed();
    }

    private void WallDestroyed()
    {
        OnWallDestroy?.Invoke();
        Destroy(gameObject);
    }
}