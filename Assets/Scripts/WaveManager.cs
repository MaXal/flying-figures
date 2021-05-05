using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private WavesDescription gameWaves;
    
    private int currentWaveIdx;
    
    public static Wave Current { get; set; }

    private void Awake()
    {
        Current = gameWaves.waves[currentWaveIdx];
    }

    public bool TryIncrementWave()
    {
        if (currentWaveIdx + 1 >= gameWaves.waves.Length) return false;
        
        currentWaveIdx++;
        Current = gameWaves.waves[currentWaveIdx];
        return true;
    }
}