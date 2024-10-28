using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameTimeFormatter
{
    string FormatTime(int hour, int minute);
}