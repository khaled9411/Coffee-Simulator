using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CafeManager : MonoBehaviour
{
    public static CafeManager instance;

    [Serializable]
    public class AreaItems
    {
        public Area area;
        public List<MonoBehaviour> items;
        public List<AirConditioner> Additions;
        public List<DepartmentAdditionsSO> Items;
    }
    // is the cofe open 
    private bool _isOpen;
    public bool isOpen { 
        get { return _isOpen; } 
        set { _isOpen = value; 
            OnIsOpenChanage?.Invoke(_isOpen); } }

    private float _overallSatisfactionRate = 1;
    public float overallSatisfactionRate { 
        get { return _overallSatisfactionRate; } 
        set { _overallSatisfactionRate = Mathf.Clamp01(value); } }

    private int _currentCustomerCount;
    public int currentCustomerCount { 
        get { return _currentCustomerCount; } 
        set { _currentCustomerCount = value; if (value < 0) Debug.Log($"The currentCustomerCount is {currentCustomerCount}"); }}

    [SerializeField] private List<AreaItems> areaItemsList;
    private int currentAreaIndex = 0;

    private GameObject[] buyableZone;

    public Action<bool> OnIsOpenChanage;
    public Action OnAreaOppened;

    public List<AreaItems> GetAreaItemsList()
    {
        return areaItemsList;
    }

    private void Awake()
    {
        instance = this;
        buyableZone = new GameObject[areaItemsList.Count];
            

        for (int i = 0; i < areaItemsList.Count; i++)
        {
            foreach (var item in areaItemsList[i].items) 
            {
                if(item is Device device)
                {
                    device.ownArea = areaItemsList[i].area;
                    device.OnPurchased += Device_OnPurchased;
                }
            }

            foreach (var ac in areaItemsList[i].Additions)
            {
                ac.ownArea = areaItemsList[i].area;
            }

            areaItemsList[i].area.acCount = areaItemsList[i].Additions.Count;
            areaItemsList[i].area.itemCount = areaItemsList[i].items.Count;

            buyableZone[i] = areaItemsList[i].area.GetComponentInChildren<BuyableInteractionZone>()?.gameObject;
            buyableZone[i]?.SetActive(false);
        }

    }

    private void Start()
    {

        Faint.Instance.onFaint += RestCafe;
        Bed.Instance.onSleep += RestCafe;

        for (int i = 1; i < areaItemsList.Count; i++)
        {
            if (areaItemsList[i].area.IsPurchased())
                currentAreaIndex++;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            isOpen = !isOpen;
        }

    }
    private void Device_OnPurchased()
    {
        areaItemsList[currentAreaIndex].area.UpdateTemperature();

        if ((currentAreaIndex + 1) >= areaItemsList.Count) return;

        foreach (var item in areaItemsList[currentAreaIndex].items)
        {
            if (item is IBuyableRespondable buyableItem && !buyableItem.IsPurchased())
                return;
        }

        buyableZone[currentAreaIndex + 1].SetActive(true);
    }

    public float GetOverallPricePercentage()
    {
        if (areaItemsList.Count == 0)
            return 0;

        float totalPercentage = 0;

        foreach (var item in areaItemsList) 
        {
            totalPercentage += item.area.pricePercentageMultiplicand;
        }

        return Mathf.Clamp(totalPercentage / areaItemsList.Count, -50, 100);
    }

    public float GetOverallTemperaturePercentage()
    {
        if (areaItemsList.Count == 0)
            return 0;

        float totalPercentage = 0;
        foreach (var item in areaItemsList)
        {
            // Remap temperature from 18-30 to 1-0
            float normalizedTemp = 1 - Mathf.InverseLerp(18f, 30f, item.area.temperature);
            totalPercentage += normalizedTemp;
        }

        return Mathf.Clamp01(totalPercentage / areaItemsList.Count);
    }
    public float GetOverallTrashPercentage()
    {
        if (TrashSpawnPoint.spawnPointsCounter == 0)
            return 1; // Avoid division by zero, return max if no spawn points

        // Normalize trash count to 0-1 range, where fewer trash items approach 1
        float normalizedTrashPercentage = 1 - (float)TrashSpawnPoint.trashCounter / TrashSpawnPoint.spawnPointsCounter;

        return Mathf.Clamp01(normalizedTrashPercentage);
    }


    public bool CanOpenNextArea()
    {

        if (currentAreaIndex < areaItemsList.Count - 1)
        {
            foreach (var item in areaItemsList[currentAreaIndex].items)
            {
                if (item is IBuyableRespondable buyableItem && !buyableItem.IsPurchased())
                    return false;
            }
        }

        return true;
    }
    
    private void RestCafe()
    {
        //isOpen = false; 
        //currentCustomerCount = 0;

        //for (int i = 0; i <= currentAreaIndex; i++)
        //{
        //    foreach (var item in areaItemsList[i].items)
        //    {
        //        if (item is IBuyableRespondable buyableItem && buyableItem.IsPurchased())
        //        {
        //            buyableItem.isAvailable = true;
        //        }
        //    }
        //}

        //CashierManager.instance.ResetCashier();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool HasAvailableSeats()
    {

        int availableItems = 0;

        for (int i = 0; i <= currentAreaIndex; i++)
        {
            foreach (var item in areaItemsList[i].items)
            {
                if (item is IBuyableRespondable buyableItem && buyableItem.IsPurchased()
                    &&  buyableItem.isAvailable)
                {
                    availableItems++;
                }
            }
        }

        return availableItems > 0 && areaItemsList[currentAreaIndex].area.GetMaxCustomerCount() > currentCustomerCount;
    }

    public Transform GetAvailableItemTransform()
    {
        if (!HasAvailableSeats())
        {
            return null;
        }

        List<Transform> availableTransforms = new List<Transform>();

        for (int i = 0; i <= currentAreaIndex; i++)
        {
            foreach (var item in areaItemsList[i].items)
            {
                if (item is IBuyableRespondable buyableItem && buyableItem.IsPurchased() && buyableItem.isAvailable)
                {
                    availableTransforms.Add((buyableItem as MonoBehaviour).transform);
                }
            }
        }

        if (availableTransforms.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableTransforms.Count);
            Transform selectedTransform = availableTransforms[randomIndex];

            (selectedTransform.GetComponent<IBuyableRespondable>()).isAvailable = false;
            currentCustomerCount++;

            if(selectedTransform.TryGetComponent<Device>(out Device device))
                return device.GetCustomerSeat();

            return selectedTransform;
        }
        return null;
    }


    public void LeaveItem(IBuyableRespondable buyableItem)
    {
        if (buyableItem != null)
        {
            currentCustomerCount--;
            buyableItem.isAvailable = true;
        }
        else
        {
            Debug.LogWarning("Attempted to leave a null item.");
        }
    }

    public void AddMoney(Device device)
    {
        if(device != null)
        {
            MoneyManager.Instance.AddMoney(UnityEngine.Random.Range(device.ownArea.GetMinPriceInTheArea(), device.ownArea.GetMaxPriceInTheArea()));
        }
    }

    //help with debugging
    public void LogAvailableItems()
    {
        Debug.Log("=== Available Items Status ===");
        for (int i = 0; i <= currentAreaIndex; i++)
        {
            foreach (var item in areaItemsList[i].items)
            {
                var buyableItem = item as IBuyableRespondable;
                if (buyableItem != null && buyableItem.IsPurchased())
                {
                    Debug.Log($"Item: {item.name} | Available: {buyableItem.isAvailable}");
                }
            }
        }
        Debug.Log("===========================");
    }


    public int GetCurrentAreaIndex()
    {
        return currentAreaIndex;
    }

    public void AddToCurrentAreaIndex()
    {
        currentAreaIndex++;
        OnAreaOppened?.Invoke();
    }

    
}
