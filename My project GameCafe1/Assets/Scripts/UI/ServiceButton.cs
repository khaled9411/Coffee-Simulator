using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ServiceButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private int areaNumber;
    [SerializeField] private GameObject lockIcon;
    private Button Button;

    private void Start()
    {
        Button = GetComponent<Button>();
        if (CafeManager.instance.GetCurrentAreaIndex() >= areaNumber)
        {
            Button.interactable = true;
            lockIcon.SetActive(true);
        }
        else
        {
            Button.interactable = false;
            lockIcon.SetActive(false);
        }
        Button.onClick.AddListener(() =>
        {
            // put ther funcino here
        });
    }
}
