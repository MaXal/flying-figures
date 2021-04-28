using UnityEngine;

public class WallsGenerator : MonoBehaviour
{
    [SerializeField] private GameObject wall;

    private void Start()
    {
        GenerateNewWall();
    }

    private void GenerateNewWall()
    {
        var generatedWall = Instantiate(wall, new Vector3(33, 0, 0), Quaternion.identity);
        generatedWall.GetComponent<Wall>().OnWallDestroy += GenerateNewWall;
    }
}