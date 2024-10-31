using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FadeEvent
{
    public static event Action OnFadeInStart;
    public static event Action OnFadeInEnd;
    public static event Action OnFadeOutStart;
    public static event Action OnFadeOutEnd;
    public static void TriggerFadeInStart()
    {
        OnFadeInStart?.Invoke();
    }
    public static void TriggerFadeInEnd()
    {
        OnFadeInEnd?.Invoke();
    }
    public static void TriggerFideOutStart()
    {
        OnFadeOutStart?.Invoke();
    }
    public static void TriggerFideOutEnd() 
    {
        OnFadeOutEnd?.Invoke();
    }
}
