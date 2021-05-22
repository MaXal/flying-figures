using TMPro;
using UnityEngine;

public class BreakingBar : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        UpdatePercent_OnPercentChanges();
        Breaking.OnPercentChanges += UpdatePercent_OnPercentChanges;
    }

    private void UpdatePercent_OnPercentChanges()
    {
        text.text = $"Break Bar: {Breaking.Percent}%";
    }
}
