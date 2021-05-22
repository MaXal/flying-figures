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
        Player.OnPlayerLostLife += OnPlayerLostLife;
    }

    private void OnPlayerLostLife()
    {
        Percent = 0 - increment;
    }

    private void Increment_OnPlayerPassedWall(GameObject _)
    {
        Percent += increment;
        OnPercentChanges?.Invoke();

        if (Percent < 100) return;
        
        OnBreaking?.Invoke();
        Percent = 0;
        OnPercentChanges?.Invoke();
    }
}
