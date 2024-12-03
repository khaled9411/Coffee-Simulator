using System;
using UnityEngine;

public class MoneyManager : MonoBehaviour, ISaveable
{
    [SerializeField]
    private string uniqueID;
    public string UniqueID
    {
        get { return uniqueID; }
        private set { uniqueID = value; }
    }

    public static MoneyManager Instance;
    private float _money;
    public float Money
    {
        get { return _money; }
        private set
        {
            _money = value;
            OnMoneyChange?.Invoke();
        }
    }

    public event Action OnMoneyChange;

    private void Awake()
    {
        Money = 5030;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddMoney(float amount)
    {
        Money += amount;
        //Debug.Log($"Added {amount} money. New total: {Money}");
    }

    public void SubtractMoney(float amount)
    {
        if (Money >= amount)
        {
            Money -= amount;
            //Debug.Log($"Subtracted {amount} money. New total: {Money}");
        }
        else
        {
            Debug.LogWarning($"Not enough money to subtract {amount}. Current money: {Money}");
        }
    }

    public void RequiredSubtractMoney(float amount)
    {
        if (Money >= amount)
        {
            Money -= amount;
            //Debug.Log($"Subtracted {amount} money. New total: {Money}");
        }
        else
        {
            Money = 0;
        }
    }

    public bool TryBuy(float amount)
    {
        if (Money >= amount)
        {
            Money -= amount;
            //Debug.Log($"Subtracted {amount} money. New total: {Money}");
            return true;
        }
        else
        {
            Debug.LogWarning($"Not enough money to subtract {amount}. Current money: {Money}");
            return false;
        }
    }

    public SaveData SaveData()
    {
        return new FloatSaveData(Money);
    }

    public void LoadData(SaveData data)
    {
        if (data is FloatSaveData floatData)
        {
            Money = floatData.value;
            Debug.Log($"Loaded money value: {Money}");
        }
    }
}