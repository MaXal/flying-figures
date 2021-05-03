using TMPro;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    private TextMeshProUGUI text;
    private float time;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (GameManager.GameState != GameState.Play) return;
        time += Time.smoothDeltaTime;
        text.text = time.ToString("F0");
    }
}