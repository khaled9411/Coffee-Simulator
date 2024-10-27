using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{

    [field: SerializeField] public string verbName { get; set; } = "Sleep";

    public void Interact()
    {
        SaveSystem.SaveGame();
        Debug.Log("you are sleeeeeepiiing");
        Debug.Log("Saveing...");
    }
}
