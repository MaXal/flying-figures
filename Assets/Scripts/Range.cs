using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Range
{
    [SerializeField] private int max;
    [SerializeField] private int min;

    public Range(int min, int max)
    {
        if (min > max) throw new ArgumentException("Max value " + max + " is lower than " + min);
        if (min < 0) throw new ArgumentOutOfRangeException(nameof(min));
        if (max < 0) throw new ArgumentOutOfRangeException(nameof(max));

        Min = min;
        Max = max;
    }

    public int Max
    {
        get => max;
        set => max = value;
    }

    public int Min
    {
        get => min;
        set => min = value;
    }

    public int RandomValue => Random.Range(Min, Max);
}