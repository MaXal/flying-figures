using UnityEngine;

public class WallsGenerator : MonoBehaviour
{
    [SerializeField] private GameObject wall;
    [SerializeField] private float initSpeed;
    [SerializeField] private float speedModifier;
    [SerializeField] private float wallInitialLocation;
    [SerializeField] private int speedDecrementOnBreak;

    private GameObject generatedWall;

    private void Start()
    {
        generatedWall = Instantiate(wall, new Vector3(wallInitialLocation, 0, 0), Quaternion.identity);
        GenerateNewWall(null);
        Player.OnPlayerPassedWall += GenerateNewWall;
        Player.OnPlayerDestroy += UnsubscribeOfPlayer_OnPlayerDestroy;
        Breaking.OnBreaking += OnBreaking;
    }

    private void UnsubscribeOfPlayer_OnPlayerDestroy()
    {
        Player.OnPlayerPassedWall -= GenerateNewWall;
    }

    private void GenerateNewWall(GameObject _)
    {
        generatedWall.GetComponent<Wall>().InitWallSpeed(initSpeed, speedModifier);
        initSpeed += speedModifier;
        generatedWall = Instantiate(wall, new Vector3(wallInitialLocation, 0, 0), Quaternion.identity);
    }

    private void OnBreaking()
    {
        initSpeed -= speedDecrementOnBreak;
    }
}