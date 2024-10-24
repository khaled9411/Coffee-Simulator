using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public interface IRespondable 
{
    public string respondableName { get; set; }

    public string verbName {  get; set; }  
    

    public void respond();
}
