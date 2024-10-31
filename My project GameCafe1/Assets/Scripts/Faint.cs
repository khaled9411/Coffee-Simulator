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

    public void StartFaint()
    {
        MoneyManager.Instance.RequiredSubtractMoney(moneyPenalty);
        FadeEvent.TriggerFade();
        SaveSystem.SaveGame();
        GameTimeManager.instance.ResetDayTime();
    }
}
