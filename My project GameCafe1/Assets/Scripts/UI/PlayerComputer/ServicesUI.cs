using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ServicesUI : MonoBehaviour
{
    public static ServicesUI Instance {  get; private set; }

    [SerializeField] private Button closeButton;
    [SerializeField] private ServiceButton cashierButton;
    [SerializeField] private ServiceButton cafeteriaWorkerButton;
    [SerializeField] private ServiceButton janitorButton;

    public Action OnBuyJanitor;

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
        cashierButton.Button.onClick.AddListener(() =>
        {
            if (MoneyManager.Instance.TryBuy(cashierButton.GetPrice()))
            {
                CashierWorker.Instance.hasCashierWorker = true;
                cashierButton.isPurchased = true;
                cashierButton.Buy();
            }
        });
        cafeteriaWorkerButton.Button.onClick.AddListener(() =>
        {
            if (MoneyManager.Instance.TryBuy(cafeteriaWorkerButton.GetPrice()))
            {
                CafeteriaWorker.Instance.hasCafeteriaWorker = true;
                cafeteriaWorkerButton.isPurchased = true;
                cafeteriaWorkerButton.Buy();
            }
        });
        janitorButton.Button.onClick.AddListener(() =>
        {
            if (MoneyManager.Instance.TryBuy(janitorButton.GetPrice()))
            {
                OnBuyJanitor?.Invoke();
                janitorButton.Buy();
            }
        });
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
