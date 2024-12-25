using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Targetable : MonoBehaviour
{
    [SerializeField] private string targetName;

    // the OutlineController of this object will listen to these events to show and hide the outline
    public event Action OnTargetableEvent;
    public event Action OnExitTargtableEvent;
    public string GetName()
    {
        return targetName;
    }
    public void SetName(string name)
    {
        this.targetName = name;
    }
    // you can put the logic that call the script on the canvas to show the name here. we make it in the interacthandler no problem 

    public void OnTargetable()
    {
        OnTargetableEvent?.Invoke();
    }
    public void OnExitTargtable()
    {
        OnExitTargtableEvent?.Invoke();
    }
}
