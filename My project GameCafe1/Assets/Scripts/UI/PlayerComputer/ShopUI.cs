using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public static ShopUI Instance { get; private set; }

    [SerializeField] private Button[] areasButtons;
    [SerializeField] private GameObject itemButtonPrefab;
    [SerializeField] private GameObject itemsButtonParent;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
        
        List<CafeManager.AreaItems> AreaItemsList = CafeManager.instance.GetAreaItemsList();
        for (int i = 0; i < AreaItemsList.Count; i++)
        {
            int index = i;
            areasButtons[i].onClick.AddListener(()=>{
                Debug.Log("pressed" + index);
                for(int j = 0; j< itemsButtonParent.transform.childCount; j++)
                {
                    Destroy(itemsButtonParent.transform.GetChild(j).gameObject);
                }
                Debug.Log("item number is " + AreaItemsList[index].ItemsSO.Count);
                for(int j = 0; j < AreaItemsList[index].ItemsSO.Count;j++) 
                {
                    AdditionsButton button = Instantiate(itemButtonPrefab, itemsButtonParent.transform).GetComponent<AdditionsButton>();
                    button.Setup(AreaItemsList[index].ItemsSO[j].price, AreaItemsList[index].ItemsSO[j].icon, AreaItemsList[index].ItemsSO[j].itemName, AreaItemsList[index].Additions[j].isPurchased);
                    int index1 = j;
                    button.button.onClick.AddListener(() =>
                    {
                        if (MoneyManager.Instance.TryBuy(AreaItemsList[index].ItemsSO[index1].price))
                        {
                            button.ShowPurchasedPanel();
                            AreaItemsList[index].Additions[index1].isPurchased = true;
                        }
                    });
                }
            });
        }
        areasButtons[0].onClick?.Invoke();
        Hide();
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
