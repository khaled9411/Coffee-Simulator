using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayPanelUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI targetNameText;
    [SerializeField] TextMeshProUGUI interactText;
    [SerializeField] TextMeshProUGUI buyablePriceText;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] Button interactButton;
    [SerializeField] Button storeButton;
    [SerializeField] Button pauseButton;
    [SerializeField] Button dropBroomButton;
    [SerializeField] Button hitWithBroomButton;


    private void Start()
    {
        HideTargetnName();
        HideInteractButton();
        HideBuyablePriceText();
        UpdateMoneyText();
        dropBroomButton.gameObject.SetActive(false);
        hitWithBroomButton.gameObject.SetActive(false);


        MoneyManager.Instance.OnMoneyChange += UpdateMoneyText;
        Player.Instance.interactHandler.OnTargetTarget += ShowTargetName;
        Player.Instance.interactHandler.OnTargetInteractbale += ShowInteractButton;
        Player.Instance.interactHandler.OnTargetBuyable += ShowBuyablePriceText;
        Player.Instance.interactHandler.OnNoTarget += InteractHandler_OnNoTarget;
        Player.Instance.interactHandler.OnPickBroom += InteractHandler_OnPickBroom;
        Player.Instance.interactHandler.OnDropBroom += InteractHandler_OnDropBroom;

        storeButton.onClick.AddListener(() =>
        {
            StopTime();
            StoreMenuUI.Instance.Show(PlayTime);
        });
        pauseButton.onClick.AddListener(() =>
        {
            StopTime();
            SettingMenuUI.Instance.Show(PlayTime);
        });
    }

    private void InteractHandler_OnDropBroom()
    {
        dropBroomButton.gameObject.SetActive(false);
        hitWithBroomButton.gameObject.SetActive(false);
    }

    private void InteractHandler_OnPickBroom()
    {
        dropBroomButton.gameObject.SetActive(true);
        hitWithBroomButton.gameObject.SetActive(true);
    }

    // it should be static function in one of the game managers
    private void StopTime()
    {
        Time.timeScale = 0;
    }
    private void PlayTime()
    {
        Time.timeScale = 1;
    }
    private void InteractHandler_OnNoTarget()
    {
        HideTargetnName();
        HideInteractButton();
        HideBuyablePriceText();
    }

    private void UpdateMoneyText()
    {
        moneyText.text = MoneyManager.Instance.Money.ToString("F2");
    }

    private void ShowTargetName(string name)
    {
        targetNameText.text = name;
        targetNameText.gameObject.SetActive(true);
    }
    private void HideTargetnName()
    {
        targetNameText.gameObject.SetActive(false);
    }


    private void ShowInteractButton(string verbName)
    {
        interactText.text = verbName;
        interactButton.gameObject.SetActive(true);
    }
    private void HideInteractButton()
    {
        interactButton.gameObject.SetActive(false);
    }


    private void ShowBuyablePriceText(float price)
    {
        buyablePriceText.text = "$ " + price.ToString();
        buyablePriceText.transform.parent.gameObject.SetActive(true);
    }
    private void HideBuyablePriceText()
    {
        buyablePriceText.transform.parent.gameObject.SetActive(false);
    }
}
