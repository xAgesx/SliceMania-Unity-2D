using System;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System.Threading;
using UnityEngine.UI;

public class RewardedAdExample : MonoBehaviour {
    private string adUnitId = "ca-app-pub-3940256099942544/5224354917";

    private RewardedAd rewardedAd;
    public GameManager gm;
    public Button rewardBtn;
    int deaths;

    // A queue to store actions to be run on the main thread.
    private static readonly Queue<Action> mainThreadActions = new Queue<Action>();

    // This method is part of Unity's game loop and runs on the main thread.
    void Update() {
        if (mainThreadActions.Count > 0) {
            // We lock the queue to prevent issues from other threads.
            lock (mainThreadActions) {
                while (mainThreadActions.Count > 0) {
                    mainThreadActions.Dequeue().Invoke();
                }
            }
        }
    }

    // A simple method to add a task to our main thread queue.
    private static void Enqueue(Action action) {
        lock (mainThreadActions) {
            mainThreadActions.Enqueue(action);
        }
    }

    void Start() {
        // Initialize the mobile ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) => {
            // After initialization, start loading the ad.
            Enqueue(() => LoadRewardedAd());
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
                // This is the important change: all the Unity-related code
                // is now enqueued to run on the main thread.
                Enqueue(() => {
                    if (error != null || ad == null) {
                        Debug.LogError("Rewarded ad failed to load with error: " + error);
                        rewardBtn.interactable = false;
                        return;
                    }

                    Debug.Log("Rewarded ad loaded successfully.");
                    rewardedAd = ad;

                    deaths = gm.getPrefDeaths();
                    if (deaths == 0) {
                        rewardBtn.interactable = true;
                        gm.deaths++;
                    } else {
                        rewardBtn.interactable = false;
                    }
                });
            });
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
        Debug.Log("rewarded");
        // We put the game logic into a function and enqueue it.
        Enqueue(() => {
            gm.resetScene();
            gm.score = PlayerPrefs.GetInt("Score");
            Time.timeScale = 1;
        });
    }
}
