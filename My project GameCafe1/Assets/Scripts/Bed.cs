using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{

    [field: SerializeField] public string verbName { get; set; } = "Sleep";

    public void Interact()
    {
        SleepEffect();
    }

    private void FadeEvent_OnFadeInEnd()
    {
        GameTimeManager.instance.ResetDayTime();
        Player.Instance.SpwanInStartPoint();
        FadeEvent.OnFadeInEnd -= FadeEvent_OnFadeInEnd;
    }
    private void OnDestroy()
    {
        FadeEvent.OnFadeInEnd -= FadeEvent_OnFadeInEnd;
    }

    private void SleepEffect()
    {
        FadeEvent.OnFadeInEnd += FadeEvent_OnFadeInEnd;
        FadeEvent.TriggerFadeInStart();
        SaveSystem.SaveGame();

        Debug.Log("you are sleeeeeepiiing");
        Debug.Log("Saveing...");
    }
}
