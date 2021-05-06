using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private const int NumberOfTiles = 9;
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
        var generatedColors = Generate(
            WaveManager.Current.ColorsRange, 
            WaveManager.Current.BlackSquaresRange, 
            WaveManager.Current.SingleColorInRowRange);
        for (var i = 0; i < NumberOfTiles; i++)
        {
            var generatedTile = Instantiate(tile, new Vector3(transform.position.x, 1 + i * 2, 0),
                Quaternion.Euler(0, 0, 90));
            generatedTile.GetComponent<SpriteRenderer>().sprite = 
                spriteManager.GetTileSprite((int) generatedColors[i]);
            generatedTile.GetComponent<Tile>().Color = generatedColors[i];
            generatedTile.transform.parent = gameObject.transform;
        }
    }

    private List<Color> Generate(Range colors, Range blackSquares, Range singleColorInRow)
    {
        (colors, blackSquares, singleColorInRow) =
            VerifyAndCorrectRanges(colors, blackSquares, singleColorInRow);

        var maxSingleColorInRow = singleColorInRow.RandomValue;
        if (colors.Min == 2) blackSquares.Min = 2 + (NumberOfTiles - 2) / (maxSingleColorInRow + 1);

        var availableColors = new List<Color> {Color.Black};
        var numberOfColors = colors.RandomValue;
        while (availableColors.Count < numberOfColors)
        {
            var color = (Color) Random.Range(0, spriteManager.GetNumberOfTileColors());
            if (!availableColors.Contains(color)) availableColors.Add(color);
        }

        var result = new List<Color>();
        for (var i = 0; i < NumberOfTiles; i++)
        {
            var randomColor = (i == 0 || i == NumberOfTiles - 1) && Random.Range(0, 10) > 0
                ? Color.Black
                : availableColors[Random.Range(0, availableColors.Count)];
            result.Add(randomColor);
        }

        var numberOfBlackTiles = blackSquares.RandomValue;
        EnsureNumberOfColorsInRow(result, availableColors, maxSingleColorInRow);
        EnsureNumberOfBlackTiles(result, availableColors, numberOfBlackTiles);

        return result;
    }

    private static (Range, Range, Range) 
        VerifyAndCorrectRanges(Range colors, Range blackSquares, Range singleColorInRow)
    {
        if (colors.Min < 2) colors.Min = 2;
        if (colors.Max > 5) colors.Max = 5;
        if (blackSquares.Min < 2) blackSquares.Min = 2;
        if (blackSquares.Max > 8) blackSquares.Max = 8;
        if (singleColorInRow.Min == 0) singleColorInRow.Min = 1;

        return (colors, blackSquares, singleColorInRow);
    }

    private static void EnsureNumberOfBlackTiles(IList<Color> result, IReadOnlyList<Color> availableColors,
        int numberOfBlackTiles)
    {
        var blackTiles = new List<int>();
        for (var i = 0; i < NumberOfTiles; i++)
            if (result[i] == Color.Black)
                blackTiles.Add(i);

        while (blackTiles.Count != numberOfBlackTiles)
        {
            var index = Random.Range(0, NumberOfTiles);
            if (blackTiles.Count < numberOfBlackTiles)
            {
                if (blackTiles.Contains(index)) continue;
                blackTiles.Add(index);
                result[index] = Color.Black;
            }
            else
            {
                if (!blackTiles.Contains(index)) continue;
                blackTiles.Remove(index);
                result[index] = availableColors[Random.Range(0, availableColors.Count)];
            }
        }
    }

    private static void EnsureNumberOfColorsInRow(IList<Color> result, IReadOnlyList<Color> availableColors,
        int maxSingleColorInRow)
    {
        var tilesInRow = 1;
        for (var i = 2; i < NumberOfTiles - 1; i++)
        {
            var previousColor = result[i - 1];
            var color = result[i];
            if (color == previousColor || color == Color.Black)
            {
                tilesInRow++;
                if (tilesInRow <= maxSingleColorInRow) continue;
                while (true)
                {
                    var newColor = availableColors[Random.Range(0, availableColors.Count)];
                    if (newColor == previousColor) continue;
                    tilesInRow = 1;
                    result[i] = newColor;
                    break;
                }
            }
            else
            {
                tilesInRow = 1;
            }
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