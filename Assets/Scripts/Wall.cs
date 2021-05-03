using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private GameObject tile;
    [SerializeField] private SpriteManager spriteManager;

    private float moveSpeed;

    private void Start()
    {
        CreateTiles();
    }

    private void Update()
    {
        if (GameManager.GameState == GameState.Pause) return;

        Move();
    }

    private void CreateTiles()
    {
        var generatedColors = Generator(new Range(1, 5), new Range(1, 3), new Range(1, 2));
        for (var i = 0; i < 9; i++)
        {
            var generatedTile = Instantiate(tile, new Vector3(transform.position.x, 1 + i * 2, 0),
                Quaternion.Euler(0, 0, 90));
            generatedTile.GetComponent<SpriteRenderer>().sprite = spriteManager.GetTileSprite((int) generatedColors[i]);
            generatedTile.GetComponent<Tile>().Color = generatedColors[i];
            generatedTile.transform.parent = gameObject.transform;
        }
    }

    private List<Color> Generator(Range colorsRange, Range blackSquaresRange, Range singleColorInRowRange)
    {
        if (colorsRange.Min < 2) colorsRange.Min = 2;
        if (colorsRange.Max > 5) colorsRange.Max = 5;
        var numberOfColors = colorsRange.RandomValue;
        var availableColors = new List<Color> {Color.Black};

        if (singleColorInRowRange.Min == 0) singleColorInRowRange.Min = 1;
        var singleColorInRow = singleColorInRowRange.RandomValue; //todo implement this

        var numberOfBlackTiles = blackSquaresRange.RandomValue;
        //todo implement restrictions based on availableColors and singleColorInRow

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
        while (blackTiles.Count < numberOfBlackTiles)
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