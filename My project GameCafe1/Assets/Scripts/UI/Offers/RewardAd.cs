using UnityEngine;
using UnityEngine.UI;

public class RewardAd : OfferUI
{
    void OnEnable()
    {
        ADHandler.Instance.LoadRewardedAD();
    }

    protected override void Start()
    {
        base.Start();
        UpdateOfferText();

        ADHandler.Instance.LoadRewardedAD();

        // Subscribe to the rewarded ad finish event
        ADHandler.Instance._OnRewardedADFinsh += OnRewardedAdFinished;
    }

    protected override void ShowOffer()
    {
        base.ShowOffer();
        UpdateOfferText();
    }

    private void UpdateOfferText()
    {
        float moneyAmount = (CafeManager.instance.GetCurrentAreaIndex() * 50) + 50f;
        offerText.text = $"{moneyAmount}$ - Ad!";
    }

    protected override void OnOfferButtonClick()
    {
        // Show rewarded ad
        ADHandler.Instance.ShowRewardedAD();
    }

    private void OnRewardedAdFinished(ADHandler adHandler)
    {
        // Reward the player
        float moneyAmount = (CafeManager.instance.GetCurrentAreaIndex() * 50) + 50f;
        // Add money to player's balance
        MoneyManager.Instance.AddMoney(moneyAmount);

        HideOffer();
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        ADHandler.Instance._OnRewardedADFinsh -= OnRewardedAdFinished;
    }
}