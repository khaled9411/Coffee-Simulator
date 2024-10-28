using UnityEngine;
using UnityEngine.UI;

public class GameTimeSettings : MonoBehaviour
{
    [SerializeField] private Dropdown formatDropdown;

    void Start()
    {
        formatDropdown.onValueChanged.AddListener(OnFormatChanged);
        OnFormatChanged(formatDropdown.value);
    }

    void OnFormatChanged(int value)
    {
        if (GameTimeManager.instance == null) return;

        if (value == 0)
            GameTimeManager.instance.SetTimeFormatter(new TwentyFourHourFormatter());
        else if (value == 1)
            GameTimeManager.instance.SetTimeFormatter(new TwelveHourFormatter());
    }
}
