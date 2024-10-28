using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwentyFourHourFormatter : IGameTimeFormatter
{
    public string FormatTime(int hour, int minute)
    {
        return $"{hour:D2}:{minute:D2}";
    }
}
