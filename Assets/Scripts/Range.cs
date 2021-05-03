using System;
using Random = UnityEngine.Random;

public class Range
{
    public Range(int min, int max)
    {
        if (min > max) throw new ArgumentException("Max value " + max + " is lower than " + min);
        if (min < 0) throw new ArgumentOutOfRangeException(nameof(min));
        if (max < 0) throw new ArgumentOutOfRangeException(nameof(max));

        Min = min;
        Max = max;
    }

    public int Max { get; set; }
    public int Min { get; set; }

    public int RandomValue => Random.Range(Min, Max);
}