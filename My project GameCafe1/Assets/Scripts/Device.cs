using System;
using UnityEngine;

public class Device : MonoBehaviour, IShowable, IBuyableRespondable, ISaveable
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
    [SerializeField]private Collider DeviceCollider;
    private bool _isPurchased = false;
    public bool isPurchased
    {
        get { return _isPurchased; }
        set
        {
            _isPurchased = value;
            if (_isPurchased)
            {
                isAvailable = true;
                Debug.Log($"Device {respondableName} purchased and now available");
            }
            OnPurchased?.Invoke();
        }
    }

    private bool _isAvailable = false;
    public bool isAvailable
    {
        get { return _isAvailable; }
        set
        {
            _isAvailable = value && isPurchased;
            Debug.Log($"Device {respondableName} availability changed to: {_isAvailable}");
        }
    }

    public event Action OnShowPreview;
    public event Action OnHidePreview;
    public event Action OnPurchased;


    public void Respond()
    {
        Debug.Log($"you bought a new {respondableName} with {price}$");
        isPurchased = true;
        isAvailable = true;
        EnableCollider();
        ShowVisuals();
        HidePreview();
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

    public void ShowPreview()
    {
        OnShowPreview?.Invoke();
    }

    public void HidePreview()
    {
        OnHidePreview?.Invoke();
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
            Debug.Log($"Loading device {respondableName}: isPurchased = {isPurchased}");

            if (isPurchased)
            {
                EnableCollider();
                ShowVisuals();
            }
            else
            {
                DisableCollider();
                HideVisuals();
            }
            HidePreview();
        }
    }

    public bool IsPurchased()
    {
        return isPurchased;
    }

    public void LogStatus()
    {
        Debug.Log($"Device {respondableName} Status - Purchased: {isPurchased}, Available: {isAvailable}");
    }
}