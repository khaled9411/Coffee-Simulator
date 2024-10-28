using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwelveHourFormatter : IGameTimeFormatter
{
    public string FormatTime(int hour, int minute)
    {
        string period = hour >= 12 ? "PM" : "AM";
        int twelveHourFormat = hour % 12;
        if (twelveHourFormat == 0) twelveHourFormat = 12;
        return $"{twelveHourFormat:D2}:{minute:D2} {period}";
    }
}
