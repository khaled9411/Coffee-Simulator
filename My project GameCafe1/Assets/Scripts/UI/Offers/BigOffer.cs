using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigOffer : OfferUI
{
    [SerializeField] private float amountOfmoney = 50000f;
    [SerializeField] private float offerPrice = 9.99f;

    protected override void Start()
    {
        base.Start();
        offerText.text = $"{offerPrice}$";
    }
    protected override void OnOfferButtonClick()
    {
        // show the IAP 
        HideOffer();
    }
}
