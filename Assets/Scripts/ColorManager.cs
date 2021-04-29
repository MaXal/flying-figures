using System;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> tileSprites;
    [SerializeField] private List<Sprite> playerSprites;

    public static Color GetColorByIndex(int index)
    {
        return index switch
        {
            0 => Color.Blue,
            1 => Color.Yellow,
            2 => Color.Red,
            3 => Color.Green,
            4 => Color.Black,
            _ => throw new ArgumentException("Invalid index")
        };
    }

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