using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardAd : OfferUI
{
    
    protected override void Start()
    {
        base.Start();
        float moneyAmount = (CafeManager.instance.GetCurrentAreaIndex() * 50) + 50f;
        offerText.text = $"{moneyAmount}$ - Ad!";
    }

    protected override void ShowOffer()
    {
        base.ShowOffer();
        float moneyAmount = (CafeManager.instance.GetCurrentAreaIndex() * 50) + 50f;
        offerText.text = $"{moneyAmount}$ - Ad!";
    }

    protected override void OnOfferButtonClick()
    {
        // show reward ad
        HideOffer();
    }
}
