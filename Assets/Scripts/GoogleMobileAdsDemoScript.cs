using UnityEngine;
using UnityEngine.SceneManagement;

using GoogleMobileAds.Api;
using GoogleMobileAds.Placement;

public class MainScene : MonoBehaviour
{
    InterstitialAdGameObject interstitialAd;

    void Start()
    {
        interstitialAd = MobileAds.Instance
            .GetAd<InterstitialAdGameObject>("interstitial");

        MobileAds.Initialize((initStatus) => {
            Debug.Log("Initialized MobileAds");
        });
    }

    public void OnClickShowGameSceneButton()
    {
        // Display an interstitial ad
        interstitialAd.ShowIfLoaded();

        // Load a scene named "GameScene"
        SceneManager.LoadScene("GameScene");
    }
}