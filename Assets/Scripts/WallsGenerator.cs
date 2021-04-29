using UnityEngine;

public class WallsGenerator : MonoBehaviour
{
    [SerializeField] private GameObject wall;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float wallInitialLocation = 33f;

    private void Start()
    {
        GenerateNewWall();
        FindObjectOfType<Player>().OnPlayerPassedWall += GenerateNewWall;
    }

    private void GenerateNewWall()
    {
        var wallObject = Instantiate(wall, new Vector3(wallInitialLocation, 0, 0), Quaternion.identity);
        var generatedWall = wallObject.GetComponent<Wall>();
        generatedWall.SetWallSpeed(moveSpeed);
    }
}