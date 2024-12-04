using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CafeteriaUI : MonoBehaviour
{
    public static CafeteriaUI Instance { get; private set; }

    [SerializeField] private Button closeButton;
    [Header("----")]
    [Space(20)]
    [SerializeField] private Button burgerButton;
    [SerializeField] private float burgerPrice;
    [SerializeField] private TextMeshProUGUI burgerQuantity;
    [Header("----")]
    [Space(20)]
    [SerializeField] private Button PizzaButton;
    [SerializeField] private float PizzaPrice;
    [SerializeField] private TextMeshProUGUI PizzaQuantity;
    [Header("----")]
    [Space(20)]
    [SerializeField] private Button FriesButton;
    [SerializeField] private float FriesPrice;
    [SerializeField] private TextMeshProUGUI FriesQuantity;
    [Header("----")]
    [Space(20)]
    [SerializeField] private Button hotdogButton;
    [SerializeField] private float hotdogPrice;
    [SerializeField] private TextMeshProUGUI hotdogQuantity;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
        Hide();
        UpdateQunatity();
        CafeteriaSystem.instance.OnQuantityChange += UpdateQunatity;

        burgerButton.onClick.AddListener(() =>
        {
            if (MoneyManager.Instance.TryBuy(burgerPrice))
            {
                CafeteriaSystem.instance.GetResources().Burger++;
                burgerQuantity.text = $"{CafeteriaSystem.instance.GetResources().Burger}";
            }
        });
        PizzaButton.onClick.AddListener(() =>
        {
            if (MoneyManager.Instance.TryBuy(PizzaPrice))
            {
                CafeteriaSystem.instance.GetResources().Pizza++;
                PizzaQuantity.text = $"{CafeteriaSystem.instance.GetResources().Pizza}";
            }
        });
        FriesButton.onClick.AddListener(() =>
        {
            if (MoneyManager.Instance.TryBuy(FriesPrice))
            {
                CafeteriaSystem.instance.GetResources().Fries++;
                FriesQuantity.text = $"{CafeteriaSystem.instance.GetResources().Fries}";
            }
        });
        hotdogButton.onClick.AddListener(() =>
        {
            if (MoneyManager.Instance.TryBuy(hotdogPrice))
            {
                CafeteriaSystem.instance.GetResources().Meat++;
                hotdogQuantity.text = $"{CafeteriaSystem.instance.GetResources().Meat}";
            }
        });

    }

    private void UpdateQunatity()
    {
        burgerQuantity.text = $"{CafeteriaSystem.instance.GetResources().Burger}";
        PizzaQuantity.text = $"{CafeteriaSystem.instance.GetResources().Pizza}";
        FriesQuantity.text = $"{CafeteriaSystem.instance.GetResources().Fries}";
        hotdogQuantity.text = $"{CafeteriaSystem.instance.GetResources().Meat}";
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
