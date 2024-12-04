using UnityEngine;

public class InterstitialAdManager : MonoBehaviour
{
    [Header("Ad Interval Settings")]
    [Tooltip("Time in minutes between interstitial ads")]
    public float adIntervalMinutes = 3f;

    private float lastAdTime;
    private bool isGameStarted = false;

    void Start()
    {
        // Initialize the last ad time when the game starts
        lastAdTime = Time.time;
        isGameStarted = true;

        // Start checking for ad display
        InvokeRepeating(nameof(CheckAndShowInterstitialAd), 60f, 60f);
    }

    void CheckAndShowInterstitialAd()
    {
        // Don't show ads if game hasn't started or ads are disabled
        if (!isGameStarted || PlayerPrefs.GetInt("ADsToggle", 1) == 0)
            return;

        // Calculate time since last ad
        float timeSinceLastAd = (Time.time - lastAdTime) / 60f;

        // Check if enough time has passed
        if (timeSinceLastAd >= adIntervalMinutes)
        {
            ShowInterstitialAd();
        }
    }

    void ShowInterstitialAd()
    {
        // Load a new interstitial ad first
        ADHandler.Instance.LoadInterstitialAD();

        // Show the interstitial ad
        ADHandler.Instance.ShowInterstitialAD();

        // Reset the last ad time
        lastAdTime = Time.time;
    }

    // Optional: Method to pause ad tracking when game is paused
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            // Game is paused, stop tracking time
            isGameStarted = false;
        }
        else
        {
            // Game resumes
            isGameStarted = true;
            lastAdTime = Time.time;
        }
    }
}