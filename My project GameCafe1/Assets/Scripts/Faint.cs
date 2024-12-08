using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Faint : MonoBehaviour
{
    public static Faint Instance;
    [SerializeField] private int moneyPenalty = 10;

    public Action onFaint;

    private void Awake()
    {
        Instance = this;
    }

    private void FadeEvent_OnFadeInEnd()
    {
        FadeEvent.OnFadeInEnd -= FadeEvent_OnFadeInEnd;
        onFaint?.Invoke();
        GameTimeManager.instance.ResetDayTime();
        Player.Instance.SpwanInStartPoint();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        FadeEvent.OnFadeInEnd -= FadeEvent_OnFadeInEnd;
    }
    public void StartFaint()
    {
        FadeEvent.ResetFadeInEnd();
        FadeEvent.OnFadeInEnd += FadeEvent_OnFadeInEnd;
        MoneyManager.Instance.RequiredSubtractMoney(moneyPenalty);
        FadeEvent.TriggerFadeInStart();
        SaveSystem.SaveGame();

    }
}
