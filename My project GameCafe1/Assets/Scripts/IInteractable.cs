using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable 
{
    public string verbName { get; set; }

    public void Interact();
}
