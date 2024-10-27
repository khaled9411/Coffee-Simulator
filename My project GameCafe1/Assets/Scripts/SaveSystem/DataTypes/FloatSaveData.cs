using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FloatSaveData : SaveData
{
    public float value;

    public FloatSaveData(float value)
    {
        this.value = value;
    }
}
