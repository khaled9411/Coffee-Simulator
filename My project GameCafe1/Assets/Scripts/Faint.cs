using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faint : MonoBehaviour
{
    public static Faint Instance;
    [SerializeField] private int moneyPenalty = 10;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        FadeEvent.OnFadeInEnd += FadeEvent_OnFadeInEnd;
    }
    private void FadeEvent_OnFadeInEnd()
    {
        GameTimeManager.instance.ResetDayTime();
        Player.Instance.SpwanInStartPoint();
    }
    private void OnDestroy()
    {
        FadeEvent.OnFadeInEnd -= FadeEvent_OnFadeInEnd;
    }
    public void StartFaint()
    {
        MoneyManager.Instance.RequiredSubtractMoney(moneyPenalty);
        FadeEvent.TriggerFadeInStart();
        SaveSystem.SaveGame();

    }
}
