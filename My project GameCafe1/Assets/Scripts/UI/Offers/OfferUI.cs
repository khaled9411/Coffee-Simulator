using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class OfferUI : MonoBehaviour
{
    [SerializeField] private float offerDureation;
    [SerializeField] private float timebetweenOffers;
    [SerializeField] protected Button offerButton;
    [SerializeField] protected TextMeshProUGUI offerText;

    protected virtual void Start()
    {
        HideOffer();
        offerButton.onClick.AddListener(OnOfferButtonClick);
    }
    protected abstract void OnOfferButtonClick();
    
    protected virtual void ShowOffer()
    {
        gameObject.SetActive(true);
        Invoke("HideOffer", offerDureation);
    }

    protected void HideOffer()
    {
        Invoke("ShowOffer", timebetweenOffers);
        gameObject.SetActive(false);
    }
}
