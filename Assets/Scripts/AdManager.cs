using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine;

public class GoogleMobileAdsDemoScript : MonoBehaviour {

    InterstitialAd interstitialAd;
    string adUnitId = "ca-app-pub-3940256099942544/1033173712";
    public void Start() {
        // Initialize Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) => {
            // This callback is called once the MobileAds SDK is initialized.
        });
        LoadAd();
    }
     public void LoadAd()
    {
        // Clean up the old ad before loading a new one.
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }

        // Create a new ad request.
        var adRequest = new AdRequest();

        // Load the interstitial ad.
        InterstitialAd.Load(adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Interstitial ad failed to load with error: " + error);
                    return;
                }
                Debug.Log("Interstitial ad loaded successfully.");
                interstitialAd = ad;
            });
    }
    public void ShowAd()
{
    if (interstitialAd != null && interstitialAd.CanShowAd())
    {
        Debug.Log("Showing interstitial ad.");
        interstitialAd.Show();
    }
    else
    {
        Debug.Log("Interstitial ad is not ready yet.");
    }
}


}