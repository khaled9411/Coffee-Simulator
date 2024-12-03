using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoTo : MonoBehaviour, IRespondable
{
    [field: SerializeField] public string respondableName { get; set; }
    [field: SerializeField] public string verbName { get; set; }

    [field: SerializeField] public Transform NewPos;

    public void Respond()
    {
        if (!NightfallManager.IsSleepTutorialDone && respondableName == "Home")
        {
            NightfallManager.IsSleepTutorialDone = true;
            PlayerPrefs.SetInt("IsSleepTutorialDone", 1);
            TutorialSystem.instance.parts[1].indicatorOnScreen.visible = false;
            TutorialSystem.instance.parts[1].indicatorOffScreen.visible = false;
        }

        FadeEvent.OnFadeInEnd += FadeEvent_OnFadeInEnd;
        FadeEvent.TriggerFadeInStart();
    }

    private void FadeEvent_OnFadeInEnd()
    {
        Player.Instance.SetPos(NewPos.position);
        FadeEvent.OnFadeInEnd -= FadeEvent_OnFadeInEnd;
    }
}
