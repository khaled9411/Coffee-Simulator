using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HungerUI : MonoBehaviour
{
    [SerializeField] private Slider hungerSlider;

    void Start()
    {
        HungerSystem.Instance.OnHungerAmountChange += UpdateHungerSlider;
        UpdateHungerSlider(HungerSystem.Instance.GetCurrentHunger());
        Debug.Log("s with OnHungerAmountChange");
    }
    private void OnDisable()
    {
        HungerSystem.Instance.OnHungerAmountChange -= UpdateHungerSlider;
    }

    private void UpdateHungerSlider(float hunger)
    {
        hungerSlider.value = hunger;
    }
}
