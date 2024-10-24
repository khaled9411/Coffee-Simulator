using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    [SerializeField] private string targetName;
    public string GetName()
    {
        return targetName;
    }
    public void SetName(string name)
    {
        this.targetName = name;
    }
}
