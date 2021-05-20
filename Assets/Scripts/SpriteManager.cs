using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> tileSprites;
    [SerializeField] private List<Sprite> playerSprites;

    public Sprite GetPlayerSprite(Color color) => playerSprites.First(s => s.name.Contains(color.ToString()));

    public Sprite GetTileSprite(int index)
    {
        return tileSprites[index];
    }

    public int GetNumberOfTileColors()
    {
        return tileSprites.Count;
    }
}