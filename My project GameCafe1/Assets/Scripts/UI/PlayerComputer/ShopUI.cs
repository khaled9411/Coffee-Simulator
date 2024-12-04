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
        for (int i = 0; i < areasButtons.Length; i++)
        {
            areasButtons[i].onClick.AddListener(()=>{ 
                for(int j = 0; j< itemsButtonParent.transform.childCount; j++)
                {
                    Destroy(itemsButtonParent.transform.GetChild(j).gameObject);
                }
                
                for(int j = 0; j< AreaItemsList.Count;j++) 
                {
                    AdditionsButton button = Instantiate(itemButtonPrefab, itemsButtonParent.transform).GetComponent<AdditionsButton>();
                    button.Setup(AreaItemsList[i].Items[j].price, AreaItemsList[i].Items[j].icon, AreaItemsList[i].Items[j].itemName, AreaItemsList[i].Additions[j].isPurchased);
                    button.button.onClick.AddListener(() =>
                    {
                        if (MoneyManager.Instance.TryBuy(AreaItemsList[i].Items[j].price))
                        {
                            button.ShowPurchasedPanel();
                            AreaItemsList[i].Additions[j].isPurchased = true;
                        }
                    });
                }
            });
        }

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
