using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FadeEvent
{
    public static event Action OnFade;

    public static void TriggerFade()
    {
        OnFade?.Invoke();
    }
}
