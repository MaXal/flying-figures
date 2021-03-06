using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    public static GameState GameState { get; set; }

    private void Start()
    {
        GameState = GameState.Play;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            GameState = GameState switch
            {
                GameState.Play => GameState.Pause,
                GameState.Pause => GameState.Play,
                _ => throw new ArgumentOutOfRangeException()
            };
        else if (Input.GetKeyDown(KeyCode.R)) RestartGame();
    }

    private void RestartGame()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}