using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirConditioner : MonoBehaviour , ISaveable
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

    public Area ownArea { get; set; }

    [SerializeField] private GameObject visual;

    public SaveData SaveData()
    {
        if (isAirConditioner)
        {
            PlayerPrefs.SetInt(UniqueID, isPurchased ? 1 : 0);
        }
        return new BoolSaveData(isPurchased);
    }

    public void LoadData(SaveData data)
    {
        if ((data is BoolSaveData boolData))
        {
            if (!isAirConditioner)
                isPurchased = boolData.value;
            else
                isPurchased = PlayerPrefs.GetInt(UniqueID, 0) == 1;
        }
    }
}
