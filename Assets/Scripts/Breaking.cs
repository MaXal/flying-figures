using System;
using UnityEngine;

public class Breaking : MonoBehaviour
{
    [SerializeField] private int increment = 5;

    public static int Percent { get; private set; }

    public static event Action OnPercentChanges;
    public static event Action OnBreaking;

    private void Start()
    {
        Player.OnPlayerPassedWall += Increment_OnPlayerPassedWall;
    }

    private void Increment_OnPlayerPassedWall(GameObject obj)
    {
        Percent += increment;
        OnPercentChanges?.Invoke();

        if (Percent < 100) return;
        
        OnBreaking?.Invoke();
        Percent = 0;
        OnPercentChanges?.Invoke();
    }
}
