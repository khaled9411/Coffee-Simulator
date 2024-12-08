using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirConditioner : MonoBehaviour, ISaveable
{
    [SerializeField]
    private string uniqueID;

    [SerializeField] private bool isAirConditioner;
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
            if (visual != null)
            {
                visual.SetActive(_isPurchased);
            }
            else
            {
                GetComponent<MeshRenderer>().enabled = _isPurchased;
            }
            if (isAirConditioner)
            {
                ownArea.UpdateTemperature();
            }
        }
    }

    [field: SerializeField] public Area ownArea { get; set; }

    [SerializeField] private GameObject visual;

    public SaveData SaveData()
    {
        return new BoolSaveData(isPurchased);
    }

    public void LoadData(SaveData data)
    {

        if ((data is BoolSaveData boolData))
        {
            isPurchased = boolData.value;
        }

    }
}
