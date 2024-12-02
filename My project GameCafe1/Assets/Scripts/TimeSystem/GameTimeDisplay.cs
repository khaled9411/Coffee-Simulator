using TMPro;
using UnityEngine;

public class GameTimeDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeDisplay;
    [SerializeField] private GameObject bedImage;

    void Start()
    {
        GameTimeManager.instance.OnTimeUpdated += UpdateTimeDisplay;
        NightfallManager.Instance.onNightTimeWarning += ShowBedImage;
        FadeEvent.OnFadeInEnd += HideBedImage;
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

    private void ShowBedImage()
    {
        bedImage.SetActive(true);
    }

    private void HideBedImage()
    {
        bedImage?.SetActive(false);
    }
}
