using TMPro;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    private bool isActive = true;
    private TextMeshProUGUI text;
    private float time;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        Player.OnPlayerDestroy += StopTime;
    }

    private void Update()
    {
        if (!isActive) return;
        time += Time.smoothDeltaTime;
        text.text = time.ToString("F0");
    }

    private void StopTime()
    {
        isActive = false;
    }
}