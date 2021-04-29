using UnityEngine;

public class WallsGenerator : MonoBehaviour
{
    [SerializeField] private GameObject wall;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float wallInitialLocation = 32f;

    private GameObject generatedWall;

    private void Start()
    {
        generatedWall = Instantiate(wall, new Vector3(wallInitialLocation, 0, 0), Quaternion.identity);
        GenerateNewWall();
        FindObjectOfType<Player>().OnPlayerPassedWall += GenerateNewWall;
    }

    private void GenerateNewWall()
    {
        generatedWall.GetComponent<Wall>().SetWallSpeed(moveSpeed);
        generatedWall = Instantiate(wall, new Vector3(wallInitialLocation, 0, 0), Quaternion.identity);
    }
}