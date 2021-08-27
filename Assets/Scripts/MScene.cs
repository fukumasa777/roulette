using UnityEngine;
using UnityEngine.SceneManagement;

using GoogleMobileAds.Api;
using GoogleMobileAds.Placement;

public class MScene : MonoBehaviour
{
    InterstitialAdGameObject interstitialAd;
    RewardedAdGameObject rewardedAdGameObject;
    void Start()
    {

        rewardedAdGameObject = MobileAds.Instance.GetAd<RewardedAdGameObject>("TestRewarded Ad");

        interstitialAd = MobileAds.Instance.GetAd<InterstitialAdGameObject>("Interstitial Ad");

        MobileAds.Initialize((initStatus) =>
        {
            Debug.Log("Initialized MobileAds");
        });
        //interstitialAd.LoadAd();
        rewardedAdGameObject.LoadAd();
    }

    public void OnClickShowSceneButton()
    {
        rewardedAdGameObject.LoadAd();
        // Display an interstitial ad
        //interstitialAd.ShowIfLoaded();
        rewardedAdGameObject.ShowIfLoaded();
        Debug.Log("広告開始");
    }

    /*
    public void OnRewardCloseBtn()
    {
        Debug.Log("動画をみた");
    }
    */
}