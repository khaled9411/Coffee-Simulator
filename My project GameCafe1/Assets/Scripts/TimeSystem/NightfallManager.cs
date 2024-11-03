using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightfallManager : MonoBehaviour
{
    private int stayingUpLateTime = 0;
    private int nightTimeWarningStart = 22;

    // Start is called before the first frame update
    void Start()
    {
        GameTimeManager.instance.OnTimeUpdated += GameTimeManager_OnTimeUpdated;
    }

    private void GameTimeManager_OnTimeUpdated(int hours, int minutes)
    {
        if (hours == nightTimeWarningStart)
        {
            Debug.Log("Warning: Approaching late-night hours!");
            //Active the bed visual
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