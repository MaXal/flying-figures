using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private GameObject tile;
    [SerializeField] private ColorManager colorManager;

    private float moveSpeed = 1f;

    private void Start()
    {
        CreateTiles();
    }

    private void Update()
    {
        Move();
    }

    private void CreateTiles()
    {
        for (var i = 0; i < 9; i++)
        {
            var randomIndex = Random.Range(0, colorManager.GetNumberOfTileColors());
            var generatedTile = Instantiate(tile, new Vector3(transform.position.x, 1 + i * 2, 0),
                Quaternion.Euler(0, 0, 90));
            generatedTile.GetComponent<SpriteRenderer>().sprite = colorManager.GetTileSprite(randomIndex);
            generatedTile.GetComponent<Tile>().Color = ColorManager.GetColorByIndex(randomIndex);
            generatedTile.transform.parent = gameObject.transform;
        }
    }

    private void Move()
    {
        var cachedTransform = transform;
        cachedTransform.position = new Vector2(cachedTransform.position.x - Time.smoothDeltaTime * moveSpeed, 0);
        if (transform.position.x < -1) WallDestroyed();
    }

    public void SetWallSpeed(float speed)
    {
        moveSpeed = speed;
    }

    private void WallDestroyed()
    {
        Destroy(gameObject);
    }
}