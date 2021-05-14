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
    private float speedModifier;

    private static float PlayerSize
    {
        get
        {
            var player = GameObject.FindGameObjectWithTag(nameof(Player));
            var scale = player.transform.localScale;
            return player.GetComponent<BoxCollider2D>().size.x * scale.x;
        }
    }

    public bool PassedByPlayer { get; private set; }

    private void Start()
    {
        Player.OnPlayerPassedWall += ApplySpeedModifier_OnPlayerPassed;
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
            var color = generatedColors[i];
            generatedTile.GetComponent<Tile>().Color = color;
            generatedTile.transform.parent = gameObject.transform;

            if (color != Color.Black) continue;
            
            var previousColored = PreviousIsColored(generatedColors, i);
            var nextColored = NextIsColored(generatedColors, i);
            
            if (previousColored || nextColored)
            {
                CorrectColliderSize(generatedTile, previousColored, nextColored);
            }
        }
    }
    
    private static bool PreviousIsColored(IReadOnlyList<Color> colors, int i)
    {
        if (i == 0) return false;
        return colors[i - 1] != Color.Black;
    }
        
    private static bool NextIsColored(IReadOnlyList<Color> colors, int i)
    {
        if (i == NumberOfTiles - 1) return false;
        return colors[i + 1] != Color.Black;
    }

    private static void CorrectColliderSize(GameObject generatedTile, bool previousColored, bool nextColored)
    {
        var myCollider = generatedTile.GetComponent<BoxCollider2D>();
        var oldSize = myCollider.size;

        if (previousColored && nextColored)
        {
            myCollider.size = new Vector2(oldSize.x - (PlayerSize * 0.95f * 2), myCollider.size.y);
        }
        else
        {
            myCollider.size = new Vector2(oldSize.x - PlayerSize * 0.95f, oldSize.y);

            var newSize = myCollider.size;
            var offset = myCollider.offset;
            myCollider.offset = previousColored 
                ? new Vector2((oldSize.x - newSize.x) * 0.5f, offset.y) 
                : new Vector2((oldSize.x - newSize.x) * -0.5f, offset.y);
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

    public void InitWallSpeed(float speed, float playerPassingModifier)
    {
        moveSpeed = speed;
        speedModifier = playerPassingModifier;
    }

    private void WallDestroyed()
    {
        Player.OnPlayerPassedWall -= ApplySpeedModifier_OnPlayerPassed;
        Destroy(gameObject);
    }

    private void ApplySpeedModifier_OnPlayerPassed(GameObject wall)
    {
        if (transform.gameObject != wall) return;
        
        PassedByPlayer = true;
        moveSpeed += speedModifier;
        Player.OnPlayerPassedWall -= ApplySpeedModifier_OnPlayerPassed;
    }
}