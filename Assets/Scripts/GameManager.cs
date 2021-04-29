using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameState GameState;

    private void Start()
    {
        GameState = GameState.Play;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameState = GameState switch
            {
                GameState.Play => GameState.Pause,
                GameState.Pause => GameState.Play
            };
        }
    }
}