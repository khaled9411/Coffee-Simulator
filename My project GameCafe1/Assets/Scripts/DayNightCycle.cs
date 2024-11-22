using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Light directionalLight;
    [SerializeField] private Gradient lightColor;
    [SerializeField] private AnimationCurve lightIntensity;

    [SerializeField] private Material daySkybox;
    [SerializeField] private Material nightSkybox;
    [SerializeField] private GameObject nightLight;
    [SerializeField] private int dayStartHour = 6;
    [SerializeField] private int nightStartHour = 18;

    private void Start()
    {
        GameTimeManager.instance.OnTimeUpdated += UpdateLighting;
    }

    private void OnDisable()
    {
        GameTimeManager.instance.OnTimeUpdated -= UpdateLighting;
    }

    private void UpdateLighting(int hour, int minute)
    {
        float timePercent = (hour * 60 + minute) / 1440f;

        directionalLight.color = lightColor.Evaluate(timePercent);
        directionalLight.intensity = lightIntensity.Evaluate(timePercent);

        float rotationAngle = Mathf.Lerp(-90f, 270f, timePercent);
        directionalLight.transform.localRotation = Quaternion.Euler(rotationAngle, 0, 0);

        if (hour > 17)
        {
            nightLight.SetActive(true);
        }
        else if (hour != 0)
        {
            nightLight.SetActive(false);
        }

        if (hour >= dayStartHour && hour < nightStartHour)
        {
            RenderSettings.skybox = daySkybox;
        }
        else
        {
            RenderSettings.skybox = nightSkybox;
        }
    }
}
