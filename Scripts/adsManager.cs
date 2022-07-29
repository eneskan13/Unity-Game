using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class adsManager : MonoBehaviour
{
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;
    public static adsManager instance;
    [SerializeField] private int rewardedValue;
    public void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return ;
        }
        RequestInterstitial();
        RequestRewarded();
    }
    #region interstitial
    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        loadInter();
    }
    public void loadInter()
    {
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }
    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        loadInter();
    }

    public void interShower()
    {
        if (interstitial.IsLoaded() == true)
            interstitial.Show();
    }
    #endregion
    #region rewardedAds
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

        this.rewardedAd = new RewardedAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        loadRewardedAds();
    }
    public void loadRewardedAds()
    {
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }
    public void showRewardedAds()
    {
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
    }
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
       
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        print("You Gain " +rewardedValue +"$");
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins")+rewardedValue);
        MenuTools.instance.cashTextSet();
        loadRewardedAds();
    }
    #endregion
}
