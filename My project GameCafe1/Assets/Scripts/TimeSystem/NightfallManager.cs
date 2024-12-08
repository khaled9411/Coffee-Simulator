using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightfallManager : MonoBehaviour
{

    public static NightfallManager Instance;

    public static bool IsSleepTutorialDone;

    private int stayingUpLateTime = 0;
    private int nightTimeWarningStart = 22;

    public Action onNightTimeWarning;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        IsSleepTutorialDone = PlayerPrefs.GetInt("IsSleepTutorialDone", 0) == 1;

        GameTimeManager.instance.OnTimeUpdated += GameTimeManager_OnTimeUpdated;
    }

    private void OnDestroy()
    {
        GameTimeManager.instance.OnTimeUpdated -= GameTimeManager_OnTimeUpdated;
    }

    bool once = true;
    private void GameTimeManager_OnTimeUpdated(int hours, int minutes)
    {
        if (hours == nightTimeWarningStart)
        {
            Debug.Log("Warning: Approaching late-night hours!");
            //Active the bed visual
            if (!IsSleepTutorialDone && once)
            {
                once = false;
                TutorialSystem.instance.OpenTutorialPopupByIndex(1);
            }
            onNightTimeWarning?.Invoke();
        }

        // Check if it's past the staying-up-late time
        if (hours == stayingUpLateTime)
        {
            Debug.Log("The player faints due to staying up too late!");
            //Disactive the bed visual

            Faint.Instance.StartFaint();
        }
    }
}
