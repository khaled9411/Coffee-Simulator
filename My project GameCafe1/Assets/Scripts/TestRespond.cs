using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRespond : MonoBehaviour, IRespondable
{
    [field: SerializeField] public string respondableName { get; set; }
    [field: SerializeField] public string verbName { get; set; }

    public void Respond()
    {
        Debug.Log("interaction zone and respond working");
    }
}
