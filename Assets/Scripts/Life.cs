using TMPro;
using UnityEngine;

public class Life : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = Player.Life.ToString();
        Player.OnPlayerLostLife += UpdateLife_OnPlayerLostLife;
    }

    private void UpdateLife_OnPlayerLostLife()
    {
        text.text = Player.Life.ToString();
    }
}