using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private TextMeshProUGUI text;
    private int score;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        Player.OnPlayerPassedWall += UpdateScore_OnPlayerPassedWall;
    }

    private void Update()
    {
        text.text = score.ToString();
    }

    private void UpdateScore_OnPlayerPassedWall(GameObject _)
    {
        score++;
    }
}