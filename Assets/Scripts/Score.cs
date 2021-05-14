using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private TextMeshProUGUI text;
    private int score;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = score.ToString();
        Player.OnPlayerPassedWall += UpdateScore_OnPlayerPassedWall;
    }

    private void UpdateScore_OnPlayerPassedWall(GameObject _)
    {
        score++;
        text.text = score.ToString();
    }
}