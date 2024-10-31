using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour, IBuyableRespondable, ISaveable
{

    [SerializeField]
    private string uniqueID;

    public string UniqueID
    {
        get { return uniqueID; }
        private set { uniqueID = value; }
    }

    [field: SerializeField] public float price { get; set; }
    [field: SerializeField] public string respondableName { get; set; }
    [field: SerializeField] public string verbName { get; set; }
    [SerializeField] private GameObject visualsParent;
    [SerializeField] private Collider DeviceCollider;
    private bool isPurchased = false;

    public event Action OnShowPreview;
    public event Action OnHidePreview;


    public void Respond()
    {
        Debug.Log($"you bought a new {respondableName} with {price}$");
        isPurchased = true;
        DisableCollider();
        HideVisuals();
    }

    private void ShowVisuals()
    {
        if (visualsParent != null)
        {
            visualsParent.SetActive(true);
        }
        else
        {
            Debug.LogWarning("there is no visual parent");
        }
    }

    private void HideVisuals()
    {
        if (visualsParent != null)
        {
            visualsParent.SetActive(false);
        }
        else
        {
            Debug.LogWarning("there is no visual parent");
        }
    }

    private void EnableCollider()
    {
        DeviceCollider.enabled = true;
    }

    private void DisableCollider()
    {
        DeviceCollider.enabled = false;
    }

    public SaveData SaveData()
    {
        return new BoolSaveData(isPurchased);
    }

    public void LoadData(SaveData data)
    {
        if (data is BoolSaveData boolData)
        {
            isPurchased = boolData.value;
            Debug.Log($"Loading Area {respondableName}: isPurchased = {isPurchased}");

            if (!isPurchased)
            {
                EnableCollider();
                ShowVisuals();
            }
            else
            {
                DisableCollider();
                HideVisuals();
            }
        }
    }

    public bool IsPurchased()
    {
        return isPurchased;
    }
}
