using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    [SerializeField] private string name;
    public string GetName()
    {
        return name;
    }
    public void SetName(string name)
    {
        this.name = name;
    }
}
