using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashierWorker : MonoBehaviour, ISaveable
{
    [SerializeField]
    private string uniqueID;

    public string UniqueID
    {
        get { return uniqueID; }
        private set { uniqueID = value; }
    }

    private bool _hasCashierWorker;
    public bool hasCashierWorker { 
        get { return _hasCashierWorker; } 

        set { _hasCashierWorker = value;
            cashierVisual?.SetActive(_hasCashierWorker);
            GetComponent<BoxCollider>().enabled = !_hasCashierWorker;
        } }


    [SerializeField] private GameObject cashierVisual;

    public void LoadData(SaveData data)
    {
        if (data is BoolSaveData boolData)
        {
            hasCashierWorker = boolData.value;
        }
    }

    public SaveData SaveData()
    {
        return new BoolSaveData(hasCashierWorker);
    }

}
