using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private TextMeshProUGUI text;
    private int score;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        FindObjectOfType<Player>().OnPlayerPassedWall += OnPlayerPassedWall;

    }

    private void Update()
    {
        text.text = score.ToString();
    }

    private void OnPlayerPassedWall()
    {
        score++;
    }
}