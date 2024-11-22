using UnityEngine;
using System;
using System.Collections;

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager instance;

    public event Action<int, int> OnTimeUpdated;

    [field: SerializeField] public int StartHour { get; set; } = 8;
    [field: SerializeField] public int StartMinute { get; set; } = 0;

    [SerializeField] private float realSecondsPerGameHour = 90f;

    private float gameTimeSpeed;
    private int gameHour;
    private int gameMinute;
    private float accumulatedTime;

    private IGameTimeFormatter timeFormatter;

    public void SetTimeFormatter(IGameTimeFormatter formatter)
    {
        timeFormatter = formatter;
    }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameHour = StartHour;
        gameMinute = StartMinute;
        gameTimeSpeed = 60f / realSecondsPerGameHour;

        timeFormatter = new TwelveHourFormatter();
    }

    void Update()
    {
        accumulatedTime += Time.deltaTime * gameTimeSpeed;

        if (accumulatedTime >= 1f)
        {
            gameMinute += (int)accumulatedTime;
            accumulatedTime %= 1f;

            if (gameMinute >= 60)
            {
                gameMinute -= 60;
                gameHour++;

                if (gameHour >= 24)
                    gameHour = 0;
            }

            OnTimeUpdated?.Invoke(gameHour, gameMinute);
        }
    }

    public void ResetDayTime()
    {
        FadeEvent.OnFadeInEnd += FadeEvent_OnFadeInEnd;
    }

    private void FadeEvent_OnFadeInEnd()
    {
        gameHour = StartHour;
        gameMinute = StartMinute;
        FadeEvent.OnFadeInEnd -= FadeEvent_OnFadeInEnd;
    }

    public string GetFormattedTime()
    {
        return timeFormatter.FormatTime(gameHour, gameMinute);
    }
}
