using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private GameObject tile;
    [SerializeField] private ColorManager colorManager;

    [Header("Generator")] [SerializeField] private int NumberOfBlackTiles = 3;
    [SerializeField] private int numberOfColors = 5;

    private float moveSpeed;

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
        var generatedColors = Generator();
        for (var i = 0; i < 9; i++)
        {
            var generatedTile = Instantiate(tile, new Vector3(transform.position.x, 1 + i * 2, 0),
                Quaternion.Euler(0, 0, 90));
            generatedTile.GetComponent<SpriteRenderer>().sprite = colorManager.GetTileSprite(generatedColors[i]);
            generatedTile.GetComponent<Tile>().Color = ColorManager.GetColorByIndex(generatedColors[i]);
            generatedTile.transform.parent = gameObject.transform;
        }
    }

    private List<int> Generator()
    {
        var availableColors = new List<int> {4};
        while (availableColors.Count < numberOfColors)
        {
            var color = Random.Range(0, colorManager.GetNumberOfTileColors());
            if (!availableColors.Contains(color)) availableColors.Add(color);
        }

        var colorIndices = new List<int>();
        var blackTiles = new List<int>();
        for (var i = 0; i < 9; i++)
        {
            var randomColor = availableColors[Random.Range(0, availableColors.Count)];
            if (randomColor == 4) blackTiles.Add(i);

            colorIndices.Add(randomColor);
        }

        //ensure number of black tiles
        while (blackTiles.Count < NumberOfBlackTiles)
        while (true)
        {
            var index = Random.Range(0, 9);
            if (blackTiles.Contains(index)) continue;
            blackTiles.Add(index);
            colorIndices[index] = 4;
            break;
        }

        return colorIndices;
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