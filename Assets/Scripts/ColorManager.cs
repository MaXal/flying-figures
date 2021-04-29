using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> tileSprites;
    [SerializeField] private List<Sprite> playerSprites;

    public Sprite GetPlayerSprite(int index)
    {
        return playerSprites[index];
    }

    public Sprite GetTileSprite(int index)
    {
        return tileSprites[index];
    }

    public int GetNumberOfTileColors()
    {
        return tileSprites.Count;
    }

    public int GetNumberOfPlayerColors()
    {
        return playerSprites.Count;
    }
}