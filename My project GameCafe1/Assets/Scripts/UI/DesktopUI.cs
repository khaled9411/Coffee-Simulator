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
        
        });

        cafeteriaButton.onClick.AddListener(() => {
        
        });

        priceListButton.onClick.AddListener(() => { 
        
        });

        shopButton.onClick.AddListener(() => { 
        
        });

        servicesButton.onClick.AddListener(() => {
        
        });
    }
}
