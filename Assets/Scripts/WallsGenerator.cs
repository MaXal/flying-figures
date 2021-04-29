using UnityEngine;

public class WallsGenerator : MonoBehaviour
{
    [SerializeField] private GameObject wall;

    private void Start()
    {
        GenerateNewWall();
        FindObjectOfType<Player>().OnPlayerPassedWall += GenerateNewWall;
    }

    private void GenerateNewWall()
    {
        Instantiate(wall, new Vector3(33, 0, 0), Quaternion.identity);
    }
}