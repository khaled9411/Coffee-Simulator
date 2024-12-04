using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broom : MonoBehaviour , IInteractable
{
    public static Broom Instance { get; private set; }

    [field: SerializeField] public string verbName { get; set; } = "Pick";
    

    private void Awake()
    {
        Instance = this;
    }
    
    public void Interact()
    {
        if (Player.Instance.interactHandler.hasBroom) return;
        Player.Instance.interactHandler.PickBroom();
    }

}
