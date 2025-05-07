using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using System;
using System.Reflection;

public class AdsManager : MonoBehaviour
{
    public bool testAds = false;
    public bool noAds = false;

    #region ids

    //    Google Play

    //App ID
    //ca-app-pub-7287038888086358~1466742873

    //Banner
    //ca-app-pub-7287038888086358/1865836732

    //Interstitial
    //ca-app-pub-7287038888086358/4812522133

    //Rewarded
    //ca-app-pub-7287038888086358/3901334526

    //    Apple Developer

    //App ID
    //ca-app-pub-7287038888086358~2773234467

    //Banner
    //ca-app-pub-7287038888086358/1986525155

    //Interstitial
    //ca-app-pub-7287038888086358/1567129859

    //Rewarded
    //ca-app-pub-7287038888086358/8252791859



    #endregion

    [Header("--------------------- Android IDs ---------------------")]
    public string bannerAndroid = "ca-app-pub-7287038888086358/1865836732";
    public string intersitialAndroid = "ca-app-pub-7287038888086358/4812522133";
    public string rewardedAndroid = "ca-app-pub-7287038888086358/3901334526";

    [Header("--------------------- IOS IDs ---------------------")]
    public string bannerIOS = "ca-app-pub-7287038888086358/1986525155";
    public string intersitialIOS = "ca-app-pub-7287038888086358/1567129859";
    public string rewardedIOS = "ca-app-pub-7287038888086358/8252791859";

    private string bannerTestIDAndroid = "ca-app-pub-3940256099942544/6300978111";
    private string bannerTestIDIOS = "ca-app-pub-3940256099942544/2934735716";
    private string intersititalTestIDAndroid = "ca-app-pub-3940256099942544/1033173712";
    private string interstitialTestIDIOS = "ca-app-pub-3940256099942544/4411468910";
    private string rewardedTestIDAndroid = "ca-app-pub-3940256099942544/5224354917";
    private string rewardedTestIDIOS = "ca-app-pub-3940256099942544/1712485313";




    string bannerID = "ca-app-pub-3940256099942544/6300978111",
        interstitialID = "ca-app-pub-3940256099942544/1033173712",
        rewardedID = "ca-app-pub-3940256099942544/5224354917";


    void SetIDs()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (testAds)
            {
                bannerID = bannerTestIDAndroid;
                interstitialID = intersititalTestIDAndroid;
                rewardedID = rewardedTestIDAndroid;
            }
            else
            {
                bannerID = bannerAndroid;
                interstitialID = intersitialAndroid;
                rewardedID = rewardedAndroid;
            }
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (testAds)
            {
                bannerID = bannerTestIDIOS;
                interstitialID = interstitialTestIDIOS;
                rewardedID = rewardedTestIDIOS;
            }
            else
            {
                bannerID = bannerIOS;
                interstitialID = intersitialIOS;
                rewardedID = rewardedIOS;
            }
        }
    }

    public static AdsManager instance;
    private void Awake()
    {


        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1) noAds = true;

        if (noAds) return;

        SetIDs();


        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });

        //LoadBannerAd();
        LoadInterstitialAd();
        LoadRewardedAd();
    }


    #region Banner


    public bool isBannerLoaded;

    BannerView _bannerView;

    /// <summary>
    /// Creates a 320x50 banner view at top of the screen.
    /// </summary>
    public void CreateBannerView()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1) noAds = true;

        if (noAds) return;

        Debug.Log("Creating banner view");

        // If we already have a banner, destroy the old one.
        if (_bannerView != null)
        {
            DestroyAd();
        }

        _bannerView = new BannerView(bannerID, AdSize.Banner, AdPosition.Bottom);


        // Create a 320x50 banner at top of the screen

    }

    /// <summary>
    /// Creates the banner view and loads a banner ad.
    /// </summary>
    public void LoadBannerAd()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1) noAds = true;

        if (noAds) return;

        // create an instance of a banner view first.
        if (_bannerView == null)
        {
            CreateBannerView();
        }

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);

        ListenToBannerAdEvents();
    }

    public void HideBannerAd()
    {
        if (_bannerView != null)
            _bannerView.Hide();
    }

    public void ShowBannerAd()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1) noAds = true;

        if (noAds) return;

        if (_bannerView != null)
        {
            _bannerView.Show();
        }
    }

    /// <summary>
    /// listen to events the banner view may raise.
    /// </summary>
    private void ListenToBannerAdEvents()
    {
        // Raised when an ad is loaded into the banner view.
        _bannerView.OnBannerAdLoaded += () =>
        {
            isBannerLoaded = true;
            HideBannerAd();
            Debug.Log("Banner view loaded an ad with response : "
                + _bannerView.GetResponseInfo());
        };
        // Raised when an ad fails to load into the banner view.
        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            isBannerLoaded = false;

            Debug.LogError("Banner view failed to load an ad with error : "
                + error);
        };
        // Raised when the ad is estimated to have earned money.
        _bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        _bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        _bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        _bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        _bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }

    /// <summary>
    /// Destroys the banner view.
    /// </summary>
    public void DestroyAd()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner view.");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }


    #endregion


    #region Intersitital

    private InterstitialAd _interstitialAd;

    /// <summary>
    /// Loads the interstitial ad.
    /// </summary>
    public void LoadInterstitialAd()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1) noAds = true;

        if (noAds) return;

        // Clean up the old ad before loading a new one.
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();


        // send the request to load the ad.
        InterstitialAd.Load(interstitialID, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                _interstitialAd = ad;
            });
    }

    /// <summary>
    /// Shows the interstitial ad.
    /// </summary>
    public void ShowInterstitialAd()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1) noAds = true;

        if (noAds) return;

        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();

            RegisterEventHandlers(_interstitialAd);
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

    private void RegisterEventHandlers(InterstitialAd interstitialAd)
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
            LoadInterstitialAd();

            Debug.Log("Interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    public void DestroyInterstitialAd()
    {
        if (_interstitialAd != null)
        {
            Debug.Log("Destroying banner view.");
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }
    }

    private void RegisterReloadHandler(InterstitialAd interstitialAd)
    {
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadInterstitialAd();
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadInterstitialAd();
        };
    }

    #endregion

    #region Rewarded

    private RewardedAd _rewardedAd;

    public bool IsRewardedAdAwailable()
    {
        if (_rewardedAd != null) return true;
        else return false;
    }

    /// <summary>
    /// Loads the rewarded ad.
    /// </summary>
    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(rewardedID, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;
            });
    }

    public void ShowRewardedAd()
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {


                // TODO: Reward the user.
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });

            RegisterEventHandlers(_rewardedAd);
        }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
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

            if (PlayerPrefs.GetInt("Reward") == 0)
            {
                GameManager.instance.reviveChance -= 1;
                GameManager.instance.gameplayUIManager.ReviveShip();

            }
            else if (PlayerPrefs.GetInt("Reward") == 1)
            {
                MainMenuManager.instance.GiveXPBoostReward();
            }

            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            if (MainMenuManager.instance != null)
            {

                MainMenuManager.instance.ShowAdUnavailableText();

            }

            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    public void DestroyRewardedAd()
    {
        if (_rewardedAd != null)
        {
            Debug.Log("Destroying banner view.");
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }
    }

    #endregion

}
