using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRespond : MonoBehaviour, IRespondable
{
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public string verbName { get; set; }

    public void respond()
    {
        Debug.Log("interaction zone and respond working");
    }
}
