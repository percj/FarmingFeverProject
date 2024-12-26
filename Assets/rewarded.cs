using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class rewarded : MonoBehaviour
{
    private RewardedAd rewardedAd;
    public void Start()
    {
        MobileAds.SetiOSAppPauseOnBackground(true);

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initstatus => { });

        RequestRewarded();
    }
    public void RequestRewarded()
    {
        string adUnitId;
#if UNITY_ANDROID
            adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
            adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        adUnitId = "unexpected_platform";
#endif
        // Clean up interstitial before using
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }
        // Initialize an InterstitialAd.
        this.rewardedAd = new RewardedAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);

        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
    }

    public void UserChoseToWatchAd()
    {
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        AudioListener.pause = true;
    }
    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        AudioListener.pause = false;

        GetComponent<banner>().RequestBanner();
        RequestRewarded();
    }
    public void HandleUserEarnedReward(object sender, Reward args)
    {
        GetComponent<banner>().DestroyBanner();
    }
}
