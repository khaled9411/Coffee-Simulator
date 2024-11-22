using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafeManager : MonoBehaviour
{
    public static CafeManager instance;

    [Serializable]
    public class AreaItems
    {
        public Area area;
        public List<MonoBehaviour> items;
    }

    private bool _isOpen;
    public bool isOpen { 
        get { return _isOpen; } 
        set { _isOpen = value; 
            OnIsOpenChanage?.Invoke(_isOpen); } }

    private float _overallSatisfactionRate = 1;
    public float overallSatisfactionRate { 
        get { return _overallSatisfactionRate; } 
        set { _overallSatisfactionRate = Mathf.Clamp01(value); } }

    [SerializeField] private List<AreaItems> areaItemsList;
    private int currentAreaIndex = 0;

    private GameObject[] buyableZone;

    public Action<bool> OnIsOpenChanage;
    public Action OnAreaOppened;
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
            buyableZone[i] = areaItemsList[i].area.GetComponentInChildren<BuyableInteractionZone>()?.gameObject;
            buyableZone[i]?.SetActive(false);
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
        if ((currentAreaIndex + 1) >= areaItemsList.Count) return;

        foreach (var item in areaItemsList[currentAreaIndex].items)
        {
            if (item is IBuyableRespondable buyableItem && !buyableItem.IsPurchased())
                return;
        }

        buyableZone[currentAreaIndex + 1].SetActive(true);
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
            //float requiredPrice = areaItemsList[currentAreaIndex + 1].area.price;
            //if (MoneyManager.Instance.TryBuy(requiredPrice))
            //{
            //    currentAreaIndex++;
            //    Debug.Log($"Opened new area: {areaItemsList[currentAreaIndex].area.respondableName}");
            //    return true;
            //}
        }

        return true;
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

        return availableItems > 0;
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

            (availableTransforms[randomIndex].GetComponent<IBuyableRespondable>()).isAvailable = false;
            return selectedTransform;
        }
        return null;
    }


    public void LeaveItem(IBuyableRespondable buyableItem)
    {
        if (buyableItem != null)
        {
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
            MoneyManager.Instance.AddMoney(UnityEngine.Random.Range(device.ownArea.minPriceInTheArea, device.ownArea.maxPriceInTheArea));
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
