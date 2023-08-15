using System;
using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK;
// using SupersonicWisdomSDK;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preload : MonoBehaviour
{

    
    // Start is called before the first frame update
    void Start()
    {
        // SupersonicWisdom.Api.AddOnReadyListener(OnSuperSonicWisdomReady);
        GameAnalytics.Initialize();
        GameAnalytics.OnRemoteConfigsUpdatedEvent += LoadMain;
    }

    // private IEnumerator Start()
    // {
    //     yield return new WaitUntil(GameAnalytics.IsRemoteConfigsReady);
    //     SceneManager.LoadSceneAsync("Main");
    // }

    private void LoadMain()
    {
        SceneManager.LoadSceneAsync("Main");
    }
}
