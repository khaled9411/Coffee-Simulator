using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Info
{
    public string title;
    public string info;
}

[CreateAssetMenu()]
public class TutorialsSO : ScriptableObject
{
    public Info[] infos;
}
