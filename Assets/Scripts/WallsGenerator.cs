using UnityEngine;

public class WallsGenerator : MonoBehaviour
{
    [SerializeField] private GameObject wall;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float speedModifier = 1f;
    [SerializeField] private float wallInitialLocation = 32f;

    private GameObject generatedWall;

    private void Start()
    {
        generatedWall = Instantiate(wall, new Vector3(wallInitialLocation, 0, 0), Quaternion.identity);
        GenerateNewWall(null);
        Player.OnPlayerPassedWall += GenerateNewWall;
        Player.OnPlayerDestroy += UnsubscribeOfPlayer_OnPlayerDestroy;
    }

    private void UnsubscribeOfPlayer_OnPlayerDestroy()
    {
        Player.OnPlayerPassedWall -= GenerateNewWall;
    }

    private void GenerateNewWall(GameObject _)
    {
        generatedWall.GetComponent<Wall>().InitWallSpeed(moveSpeed, speedModifier);
        moveSpeed += speedModifier;
        generatedWall = Instantiate(wall, new Vector3(wallInitialLocation, 0, 0), Quaternion.identity);
    }
}