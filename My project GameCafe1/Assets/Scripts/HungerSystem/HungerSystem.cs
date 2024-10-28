using System;
using System.Collections;
using UnityEngine;

public class HungerSystem : MonoBehaviour, IHungerSystem, ISaveable
{
    [SerializeField] private string uniqueID;
    public string UniqueID
    {
        get { return uniqueID; }
        private set { uniqueID = value; }
    }

    [SerializeField] private float maxHunger = 100f;
    [SerializeField] private float hungerDecreaseRate = 1f;
    private float currentHunger;

    public static HungerSystem Instance;

    public Action<float> OnEatFood;
    public Action<float> OnHungerAmountChange;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(HungerDecay());
        OnEatFood += IncreaseHunger;

        if (currentHunger <= 0)
            currentHunger = 100;
    }

    public float GetCurrentHunger()
    {
        return currentHunger;
    }

    public float GetMaxHunger()
    {
        return maxHunger;
    }

    private IEnumerator HungerDecay()
    {
        while (true)
        {
            ReduceHungerOverTime();
            yield return new WaitForSeconds(1f);
        }
    }

    public void ReduceHungerOverTime()
    {
        currentHunger -= hungerDecreaseRate;
        if (currentHunger < 0) currentHunger = 0;

        OnHungerAmountChange?.Invoke(currentHunger);
        Debug.Log($"Current Hunger: {currentHunger}");
    }

    public void IncreaseHunger(float amount)
    {
        currentHunger += amount;
        if (currentHunger > maxHunger) currentHunger = maxHunger;

        OnHungerAmountChange?.Invoke(currentHunger);
        Debug.Log($"Hunger Increased: {currentHunger}");
    }

    public SaveData SaveData()
    {
        return new FloatSaveData(currentHunger);
    }

    public void LoadData(SaveData data)
    {
        if (data is FloatSaveData floatData)
        {
            currentHunger = floatData.value;

            if (currentHunger <= 0)
                currentHunger = 10;

            Debug.Log($"Loaded hunger value: {currentHunger}");
        }
    }

    void OnDestroy()
    {
        OnEatFood -= IncreaseHunger;
    }
}
