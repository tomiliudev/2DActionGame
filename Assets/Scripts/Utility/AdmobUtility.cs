using System;
using GoogleMobileAds.Api;

public sealed class AdmobUtility : SingletonMonoBehaviour<AdmobUtility>
{
#if UNITY_ANDROID
    public readonly string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
    public readonly string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
    public readonly string adUnitId = "unexpected_platform";
#endif

    bool isInitialized = false;
    InterstitialAd interstitial;

    public void InitializeAdmob()
    {
        if (isInitialized) return;
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { isInitialized = true; });
    }

    public void RequestInterstitial()
    {
        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);

        // Called when an ad request has successfully loaded.
        interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        //interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        interstitial.LoadAd(request);
    }

    public void Show()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
    }

    public void Destroy()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Destroy();
        }
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {

    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {

    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {

    }

    private Action onAdClosed = null;
    public Action OnAdClosed
    {
        get { return onAdClosed; }
        set { onAdClosed = value; }
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        Destroy();
        if (OnAdClosed != null) onAdClosed();
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {

    }
}
