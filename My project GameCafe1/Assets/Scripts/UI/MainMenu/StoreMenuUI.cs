using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreMenuUI : MonoBehaviour
{
    public static StoreMenuUI Instance { get; private set; }

    [SerializeField] private Button closeButton;

    private Action onCloseButtonAction;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Hide();
        closeButton.onClick.AddListener(() => {
            if (onCloseButtonAction != null)
            {
                onCloseButtonAction();
            }
            Hide();
        });
    }
    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction = onCloseButtonAction;
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
