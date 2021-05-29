using System;
using UnityEngine;

[Serializable]
public class Wave
{
    [SerializeField] public Range ColorsRange;
    [SerializeField] public Range ColoredTilesNumberRange;
    [SerializeField] public Range SingleColorInRowRange;
    [SerializeField] public float WallSpeedToIncrementWave;
}