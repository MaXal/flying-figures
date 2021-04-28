using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
        Player.OnPlayerDestroy += ActivateGameOverUI;
    }

    private void ActivateGameOverUI()
    {
        gameObject.SetActive(true);
    }
}