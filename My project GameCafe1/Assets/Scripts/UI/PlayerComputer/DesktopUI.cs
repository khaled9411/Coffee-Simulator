using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DesktopUI : MonoBehaviour
{
    [SerializeField] private Button electronicStoreButton;
    [SerializeField] private Button cafeteriaButton;
    [SerializeField] private Button priceListButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button servicesButton;

    private void Start()
    {
        electronicStoreButton.onClick.AddListener(() => {
            EloctronicsStoreUI.Instance.Show();
        });

        cafeteriaButton.onClick.AddListener(() => {
            CafeteriaUI.Instance.Show();
        });

        priceListButton.onClick.AddListener(() => { 
            PriceListUI.Instance.Show();
        });

        shopButton.onClick.AddListener(() => { 
            ShopUI.Instance.Show();
        });

        servicesButton.onClick.AddListener(() => {
            ServicesUI.Instance.Show();
        });
    }
}
