using System;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private WavesDescription gameWaves;
    
    private int currentWaveIdx;
    
    public static Wave Current { get; private set; }

    private void Awake()
    {
        Current = gameWaves.waves[currentWaveIdx];
    }

    private void Start()
    {
        WallsGenerator.OnSpeedLimitReached += WaveManger_OnSpeedLimitReached;
    }

    private bool TryIncrementWave()
    {
        if (currentWaveIdx + 1 >= gameWaves.waves.Length) return false;
        
        currentWaveIdx++;
        Current = gameWaves.waves[currentWaveIdx];
        return true;
    }

    private void WaveManger_OnSpeedLimitReached()
    {
        TryIncrementWave();
    }

    private void OnDestroy()
    {
        WallsGenerator.OnSpeedLimitReached -= WaveManger_OnSpeedLimitReached;
    }
}