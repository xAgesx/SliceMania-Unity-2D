using System;
using UnityEngine;
using GoogleMobileAds.Api;
using System.Threading;
using UnityEngine.UI;

public class RewardedAdExample : MonoBehaviour {
    private string adUnitId = "ca-app-pub-3420353350305430/5087537388";

    private RewardedAd rewardedAd;
    public GameManager gm;
    public Button rewardBtn;
    void Start() {
        // Initialize the mobile ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) => {
            // After initialization, start loading the ad.
            LoadRewardedAd();
        });
    }
    public void LoadRewardedAd() {

        if (rewardedAd != null) {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");


        var adRequest = new AdRequest();

        RewardedAd.Load(adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) => {

                if (error != null || ad == null) {
                    Debug.LogError("Rewarded ad failed to load with error: " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded successfully.");
                rewardedAd = ad;


            });
        int deaths = PlayerPrefs.GetInt("Deaths", 0);
        if (deaths == 0) {
            rewardBtn.interactable = true;
            gm.deaths++;
        } else {
            rewardBtn.interactable = false;
        }
    }

    public void ShowRewardedAd() {
        if (rewardedAd != null && rewardedAd.CanShowAd()) {
            Debug.Log("Showing rewarded ad.");
            rewardedAd.Show(userEarnedRewardCallback);
        } else {
            Debug.LogError("Rewarded ad is not ready yet.");
        }
    }

    private void userEarnedRewardCallback(Reward reward) {
        Debug.Log("rewarded"); ;
        gm.resetScene();
        gm.score = PlayerPrefs.GetInt("Score");
        Time.timeScale = 1;
    }


}
