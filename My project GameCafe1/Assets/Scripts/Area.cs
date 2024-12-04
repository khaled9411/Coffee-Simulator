using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
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
    public bool isAvailable { get; set; } = false;

    [SerializeField] private GameObject visualsParent;
    [SerializeField] private Collider DeviceCollider;
    private bool isPurchased = false;

    [SerializeField] private float minPriceInTheArea;
    [SerializeField] private float maxPriceInTheArea;
    [SerializeField] private int maxCustomerCount;


    public int itemCount { get; set; }
    public int acCount { get; set; }

    public float GetMinPriceInTheArea()
    {
        return minPriceInTheArea + (minPriceInTheArea * (pricePercentageMultiplicand / 100f));
    }

    public float GetMaxPriceInTheArea() 
    {
        return maxPriceInTheArea + (maxPriceInTheArea * (pricePercentageMultiplicand / 100f));
    }

    public int GetMaxCustomerCount()
    {
        return maxCustomerCount;
    }
    public void SetMaxCustomerCount(int maxCustomerCount)
    {
        this.maxCustomerCount = maxCustomerCount;
    }

    private float _pricePercentageMultiplicand;
    public float pricePercentageMultiplicand
    { 
        get { return _pricePercentageMultiplicand; } 
        set {
            _pricePercentageMultiplicand = Mathf.Clamp(value, -50, 100); 
        } }

    public int temperature { get; set; }

    public void Respond()
    {
        Debug.Log($"you bought a new {respondableName} with {price}$");
        CafeManager.instance.AddToCurrentAreaIndex();
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

    public void UpdateTemperature()
    {

        if (acCount == 0)
        {
            temperature = 40;
        }
        else
        {
            float threshold = (float)itemCount / (acCount + 1);

            if (threshold < itemCount * 0.33f)
            {
                temperature = 18;
            }
            else if (threshold < itemCount * 0.66f)
            {
                temperature = 25;
            }
            else
            {
                temperature = 30;
            }
        }

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
