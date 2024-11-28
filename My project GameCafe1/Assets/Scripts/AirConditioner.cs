using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirConditioner : MonoBehaviour , ISaveable
{
    [SerializeField]
    private string uniqueID;

    public string UniqueID
    {
        get { return uniqueID; }
        private set { uniqueID = value; }
    }
    private bool _isPurchased;
    public bool isPurchased
    {
        get { return _isPurchased; }

        set
        {
            _isPurchased = value;
            visual?.SetActive(_isPurchased);
        }
    }

    [SerializeField] private GameObject visual;

    public SaveData SaveData()
    {
        return new BoolSaveData(isPurchased);
    }

    public void LoadData(SaveData data)
    {
        if (data is BoolSaveData boolData)
        {
            isPurchased = boolData.value;
        }
    }
}
