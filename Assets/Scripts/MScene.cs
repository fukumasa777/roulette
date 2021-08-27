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
        // Display an interstitial ad
        //interstitialAd.ShowIfLoaded();
        rewardedAdGameObject.ShowIfLoaded();

    }

    /*
    public void OnRewardCloseBtn()
    {
        Debug.Log("動画をみた");
    }
    */
}