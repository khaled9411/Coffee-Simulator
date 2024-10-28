using TMPro;
using UnityEngine;

public class GameTimeDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeDisplay;

    void Start()
    {
        if (GameTimeManager.instance != null)
        {
            GameTimeManager.instance.OnTimeUpdated += UpdateTimeDisplay;
        }
    }
    private void OnEnable()
    {
        if (GameTimeManager.instance != null)
        {
            GameTimeManager.instance.OnTimeUpdated += UpdateTimeDisplay;
        }
    }
    void OnDisable()
    {
        if (GameTimeManager.instance != null)
        {
            GameTimeManager.instance.OnTimeUpdated -= UpdateTimeDisplay;
        }
    }

    private void UpdateTimeDisplay(int hour, int minute)
    {
        timeDisplay.text = GameTimeManager.instance.GetFormattedTime();
    }
}
