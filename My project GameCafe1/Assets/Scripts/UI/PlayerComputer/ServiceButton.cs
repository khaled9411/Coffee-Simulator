using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ServiceButton : MonoBehaviour , ISaveable 
{
    [SerializeField] private float price;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private int areaNumber;
    [SerializeField] private TextMeshProUGUI areaText;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private GameObject purchasedIcon;
    public Button Button { get; private set; }

    public bool isPurchased { get; set; } = false;

    [SerializeField]
    private string uniqueID;
    public string UniqueID
    {
        get { return uniqueID; }
        private set { uniqueID = value; }
    }

    private void Awake()
    {
        Button = GetComponent<Button>();
    }

    private void Start()
    {
        CafeManager.instance.OnAreaOppened += CafeManager_OnAreaOppent;

        areaText.text = $"Open Area {areaNumber + 1}"; // this one to start the cound from 1 to the user

       


        CafeManager_OnAreaOppent();
        
    }
    public void Buy()
    {
        ShowPurchasedIcon();
        HideLockIcon();
        Button.interactable = false;
    }
    private void CafeManager_OnAreaOppent()
    {
        if (isPurchased)
        {
            ShowPurchasedIcon();
            HideLockIcon();
            Button.interactable = false;
            return;
        }

        if (CafeManager.instance.GetCurrentAreaIndex() >= areaNumber)
        {
            Button.interactable = true;
            HideLockIcon();
            HidePurchasedIcon();
        }
        else
        {
            Button.interactable = false;
            ShowLockIcon();
            HidePurchasedIcon();
        }
    }
    private void ShowPurchasedIcon()
    {
        purchasedIcon.SetActive(true);
    }
    private void HidePurchasedIcon()
    {
        purchasedIcon.SetActive(false);
    }

    private void ShowLockIcon()
    {
        lockIcon.SetActive(true);
    }
    private void HideLockIcon() 
    {  
        lockIcon.SetActive(false);
    }
    public float GetPrice()
    {
        return price;
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
        }
    }
}
