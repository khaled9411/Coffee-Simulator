using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADHandler : MonoBehaviour
{
    public static ADHandler Instance;

    //Events
    public event Action<ADHandler> _OnRewardedADFinsh;

    //buy AD Blocker
    private bool paidAD = false;
    private const string paidADPlayerPrefs = "paidAd";

    //ADs IDs
    private const string bannerID = "ca-app-pub-3940256099942544/6300978111";
    private const string interstitialID = "ca-app-pub-3940256099942544/1033173712";
    private const string rewardedID = "ca-app-pub-3940256099942544/5224354917";

    //ADs
    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;

    private void Awake()
    {
        //Set Paid AD bool
        paidAD = PlayerPrefs.GetInt(paidADPlayerPrefs, 0) > 0 ? true : false;

        if (Instance != null && Instance != this)
        {
            Debug.LogError("There is more than AD Handler !!");
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("ADs Initialised");
        });

        AdRequest adRequest = new AdRequest();
    }


    #region Buy AD Blocker
    public void SetPaidAD(bool paidAD)
    {
        this.paidAD = paidAD;

        PlayerPrefs.SetInt(paidADPlayerPrefs, paidAD ? 1 : 0);
    }

    public bool GetPaidAD()
    {
        return paidAD;
    }
    #endregion

    #region Banner

    public void LoadBannerAD()
    {
        if (PlayerPrefs.GetInt("ADsToggle", 1) == 0) return;
        CreateBannerView();
        ListenToBannerEvents();

        if (bannerView == null)
        {
            CreateBannerView();

        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        Debug.Log("Loading banner...");
        bannerView.LoadAd(adRequest);
    }

    private void CreateBannerView()
    {
        if (PlayerPrefs.GetInt("ADsToggle", 1) == 0) return;
        if (bannerView != null)
        {
            DistroyBannerAD();
        }

        bannerView = new BannerView(bannerID, AdSize.Banner, AdPosition.Top);
    }
    private void ListenToBannerEvents()
    {
        // Raised when an ad is loaded into the banner view.
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + bannerView.GetResponseInfo());
        };
        // Raised when an ad fails to load into the banner view.
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);
        };
        // Raised when the ad is estimated to have earned money.
        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Banner view paid {0} {1}." +
                adValue.Value +
                adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }

    public void DistroyBannerAD()
    {
        if (bannerView != null)
        {
            Debug.Log("Distroying Banner...");

            bannerView.Destroy();
            bannerView = null;
        }
    }

    #endregion

    #region Interstitial

    public void LoadInterstitialAD()
    {
        //if player buight the ad blocker don't load the ad
        if (PlayerPrefs.GetInt("ADsToggle", 1) == 0) return;

        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        InterstitialAd.Load(interstitialID, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.Log($"Interstitial ad faild to load {error}");
                return;
            }

            Debug.Log($"Interstitial ad loaded {ad.GetResponseInfo()}");

            interstitialAd = ad;
            InterstitialEvent(interstitialAd);
        });

    }
    public void ShowInterstitialAD()
    {
        if (PlayerPrefs.GetInt("ADsToggle", 1) == 0) return;
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("interstitial AD Not Ready yet");
        }
    }
    public void InterstitialEvent(InterstitialAd interstitialAd)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }
    #endregion

    #region Rewarded
    public void LoadRewardedAD()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        RewardedAd.Load(rewardedID, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.Log($"Rewarded AD Failed To load {error}");
                return;
            }

            Debug.Log("Rewarded AD Loaded");
            rewardedAd = ad;
            RewardedADEvents(rewardedAd);

        });
    }
    public void ShowRewardedAD()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log("Give Reward To Player");
                _OnRewardedADFinsh?.Invoke(this);
            });
        }
        else
        {
            Debug.Log("Rewarded AD Not Ready Yet");
        }
    }
    public void RewardedADEvents(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log($"Rewarded ad paid {adValue.Value} {adValue.CurrencyCode}.");
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }
    #endregion
}
