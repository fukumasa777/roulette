using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using GoogleMobileAds.Placement;

public class MainGoogleMobileAdsDemoScriptScene : MonoBehaviour
{

    void Start()
    {
        // Initialize the Mobile Ads SDK.
        MobileAds.Initialize((initStatus) =>
        {
            // SDK initialization is complete
        });

    }

    
}