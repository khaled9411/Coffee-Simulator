using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoolSaveData : SaveData
{
    public bool value;

    public BoolSaveData(bool value)
    {
        this.value = value;
    }
}
