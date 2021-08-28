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


        MobileAds.Initialize((initStatus) =>
        {
            Debug.Log("Initialized MobileAds");
        });
        //interstitialAd.LoadAd();
    }

    public void OnClickShowSceneButton()
    {
        if (GameManager.I.isRouletteStart)
        {
            return;
        }
        if (GameManager.I.stamina < 5)
        {
            rewardedAdGameObject.LoadAd();
            rewardedAdGameObject.ShowIfLoaded();
            Debug.Log("広告開始");
        }
        else
        {
            return;
        }
        // Display an interstitial ad
        
    }

    /*
    public void OnRewardCloseBtn()
    {
        Debug.Log("動画をみた");
    }
    */
}