using System;
using UnityEngine;

public class MoneyManager : MonoBehaviour, ISaveable
{
    public static MoneyManager Instance;
    private float _money;
    public float Money
    {
        get { return _money; }
        private set
        {
            _money = value;
            Debug.Log($"Money updated: {_money}");
            OnMoneyChange?.Invoke();
        }
    }

    public event Action OnMoneyChange;

    private void Awake()
    {
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
        Debug.Log($"Added {amount} money. New total: {Money}");
    }

    public void SubtractMoney(float amount)
    {
        if (Money >= amount)
        {
            Money -= amount;
            Debug.Log($"Subtracted {amount} money. New total: {Money}");
        }
        else
        {
            Debug.LogWarning($"Not enough money to subtract {amount}. Current money: {Money}");
        }
    }

    public bool TryBuy(float amount)
    {
        if (Money >= amount)
        {
            Money -= amount;
            Debug.Log($"Subtracted {amount} money. New total: {Money}");
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