using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdditionsButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject purchasedPanel;
    public Button button;

    public void Setup(float price, Sprite sprite, string name,bool isPurchased)
    {
        priceText.text = $"{price}$";
        image.sprite = sprite;
        nameText.text = name;
        if(isPurchased)
        {
            ShowPurchasedPanel();
        }
        else
        {
            HidepurchasedPanel();
        }
        button.interactable = !isPurchased;
    }
    public void ShowPurchasedPanel()
    {
        purchasedPanel.SetActive(true);
    }
    public void HidepurchasedPanel()
    {
        purchasedPanel.SetActive(false);

    }
}
