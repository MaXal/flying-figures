using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
            WaveManager.Current.ColoredTilesNumberRange, 
            WaveManager.Current.SingleColorInRowRange,
            true);
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

    private List<Color> Generate(Range colors, Range coloredTilesNumber, Range singleColorInRow, bool wallIsMonochrome = false)
    { 
        VerifyRanges(colors, coloredTilesNumber, singleColorInRow);

        var maxSingleColorInRow = singleColorInRow.RandomValue;

        var availableColors = GetAvailableColors(colors, wallIsMonochrome);

        var result = new List<Color>();
        for (var i = 0; i < NumberOfTiles; i++)
        {
            var randomColor = (i == 0 || i == NumberOfTiles - 1) && Random.Range(0, 10) > 0
                ? Color.Black
                : availableColors[Random.Range(0, availableColors.Count)];
            result.Add(randomColor);
        }

        var numberOfColoredTiles = coloredTilesNumber.RandomValue;
        EnsureNumberOfColorsInRow(result, availableColors, maxSingleColorInRow);
        EnsureNumberOfColoredTiles(result, availableColors, numberOfColoredTiles);

        return result;
    }

    private List<Color> GetAvailableColors(Range colors, bool wallIsMonochrome)
    {
        var availableColors = new List<Color> {Color.Black};
        var numberOfColors = wallIsMonochrome ? 2 : colors.RandomValue;
        while (availableColors.Count < numberOfColors)
        {
            var color = (Color) Random.Range(0, spriteManager.GetNumberOfTileColors());
            if (!availableColors.Contains(color)) availableColors.Add(color);
        }

        return availableColors;
    }

    private static void VerifyRanges(Range colors, Range coloredTiles, Range singleColorInRow)
    {
        if (colors.Min < 1) throw new Exception("Colors number cant be less than 1");
        if (colors.Max > 5) throw new Exception("Colors number cant be more than 5");
        if (coloredTiles.Min < 1) throw new Exception("The colored tiles number cant be less than 1");
        if (coloredTiles.Max > NumberOfTiles) 
            throw new Exception($"The colored tiles number cant be more than {NumberOfTiles}");
        if (singleColorInRow.Min < 1) throw new Exception("Single colors in a row cant be less than 1");
    }

    private static void EnsureNumberOfColoredTiles(IList<Color> result, IReadOnlyList<Color> availableColors,
        int numberOfColoredTiles)
    {
        var coloredTilesIndexes = new List<int>();
        for (var i = 0; i < NumberOfTiles; i++)
            if (result[i] != Color.Black)
                coloredTilesIndexes.Add(i);

        while (coloredTilesIndexes.Count != numberOfColoredTiles)
        {
            var index = Random.Range(0, NumberOfTiles);
            if (coloredTilesIndexes.Count < numberOfColoredTiles)
            {
                if (coloredTilesIndexes.Contains(index)) continue;
                coloredTilesIndexes.Add(index);
                result[index] = availableColors[Random.Range(0, availableColors.Count)];
            }
            else
            {
                if (!coloredTilesIndexes.Contains(index)) continue;
                coloredTilesIndexes.Remove(index);
                result[index] = Color.Black;
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