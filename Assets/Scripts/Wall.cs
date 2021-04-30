using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Wall : MonoBehaviour
{
    [SerializeField] private GameObject tile;
    [SerializeField] private SpriteManager spriteManager;

    [Header("Generator")] [SerializeField] private int NumberOfBlackTiles = 3;
    [SerializeField] private int numberOfColors = 5;

    private float moveSpeed;

    private void Start()
    {
        CreateTiles();
    }

    private void Update()
    {
        if (GameManager.GameState != GameState.Play) return;
        
        Move();
    }

    private void CreateTiles()
    {
        var generatedColors = Generator();
        for (var i = 0; i < 9; i++)
        {
            var generatedTile = Instantiate(tile, new Vector3(transform.position.x, 1 + i * 2, 0),
                Quaternion.Euler(0, 0, 90));
            generatedTile.GetComponent<SpriteRenderer>().sprite = spriteManager.GetTileSprite((int) generatedColors[i]);
            generatedTile.GetComponent<Tile>().Color = generatedColors[i];
            generatedTile.transform.parent = gameObject.transform;
        }
    }

    private List<Color> Generator()
    {
        var availableColors = new List<Color> {Color.Black};
        while (availableColors.Count < numberOfColors)
        {
            var color = (Color) Random.Range(0, spriteManager.GetNumberOfTileColors());
            if (!availableColors.Contains(color)) availableColors.Add(color);
        }

        var result = new List<Color>();
        var blackTiles = new List<int>();
        for (var i = 0; i < 9; i++)
        {
            var randomColor = availableColors[Random.Range(0, availableColors.Count)];
            if (randomColor == Color.Black) blackTiles.Add(i);

            result.Add(randomColor);
        }

        //ensure number of black tiles
        while (blackTiles.Count < NumberOfBlackTiles)
        while (true)
        {
            var index = Random.Range(0, 9);
            if (blackTiles.Contains(index)) continue;
            blackTiles.Add(index);
            result[index] = Color.Black;
            break;
        }

        return result;
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