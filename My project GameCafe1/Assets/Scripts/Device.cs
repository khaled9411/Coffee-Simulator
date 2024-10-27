using System;
using UnityEngine;

public class Device : MonoBehaviour, IShowable, IBuyableRespondable, ISaveable
{
    [field: SerializeField] public float price { get; set; }
    [field: SerializeField] public string respondableName { get; set; }
    [field: SerializeField] public string verbName { get; set; }
    [SerializeField] private GameObject visualsParent;
    private Collider DeviceCollider;
    private bool canBuy = false;
    private bool isPurchased = false;

    public event Action OnShowPreview;
    public event Action OnHidePreview;

    private void Start()
    {
        DeviceCollider = GetComponent<Collider>();
        DisableCollider();
        HideVisuals();
        HidePreview();
    }

    public void respond()
    {
        if (isPurchased)
        {
            EnableCollider();
            ShowVisuals();
            HidePreview();
            return;
        }

        MoneyManager.Instance.SubtractMoney(price, out canBuy);
        if (canBuy)
        {
            Debug.Log($"you bought a new {respondableName} with {price}$");
            isPurchased = true;
            EnableCollider();
            ShowVisuals();
            HidePreview();
        }
        canBuy = false;
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
        }
    }
}