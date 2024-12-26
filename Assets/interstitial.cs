using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class interstitial : MonoBehaviour
{
    private InterstitialAd interstitialAd;
    void Start()
    {
        MobileAds.SetiOSAppPauseOnBackground(true);

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initstatus => { });

        RequestInterstitial();
    }
    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif
        // Clean up interstitial before using
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }
        // Initialize an InterstitialAd.
        this.interstitialAd = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitialAd.LoadAd(request);


        // Called when the ad is closed.
        this.interstitialAd.OnAdClosed += HandleOnAdClosed;
    }

    public void UserChoseToWatchAd()
    {
        if (interstitialAd.IsLoaded())
            interstitialAd.Show();
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        RequestInterstitial();
    }
}
