﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;

public class AdManager : MonoBehaviour
{
    public Main main;
    public GameManager gameManager;

    public UnityEvent merchPopUp = new UnityEvent();
    public UnityEvent removeAdsPopUp = new UnityEvent();
    

    void Awake()
    {
        main = GameObject.FindGameObjectWithTag("Main").GetComponent<Main>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        //StartCoroutine(ShowBannerWhenInitialized());
    }

    IEnumerator ShowBannerWhenInitialized () 
    {
        while (!Advertisement.isInitialized) {
            yield return new WaitForSeconds(0.5f);
        }
        

        Advertisement.Banner.Show("Banner");
        Advertisement.Banner.SetPosition (BannerPosition.TOP_CENTER);

    }

    public bool PlayAd() 
    {
        Debug.Log("Play ad");
        Debug.Log("main.plays_since_ad " + main.plays_since_ad);
        Debug.Log("main.remove_ads " + main.remove_ads);
        Debug.Log("main.plays_between_ads " + main.plays_between_ads);


        if (main.remove_ads == false) {
            Debug.Log("Play ad 1");
            main.plays_since_ad += 1;
            if (main.plays_since_ad >= main.plays_between_ads) 
            {
                Debug.Log("Play ad 2");

                main.plays_since_ad = 0;

                if (main.nextAdType == "Ad") { 
                    Debug.Log("Play ad 3");

                    if (Advertisement.IsReady("Interstitial"))
                    {
                        Debug.Log("Play ad 4");

                        var options = new ShowOptions { resultCallback = HandleShowResult };
                        Advertisement.Show("Interstitial", options);
                        main.nextAdType = "PopUp"; 
                        Debug.Log("Play ad 7");
                    } 
                    else 
                    {
                        Debug.Log("Play ad 5");

                        main.nextAdType = "PopUp"; 
                        main.plays_since_ad = main.plays_between_ads;
                        return false;
                    } 
                }
                else if (main.nextAdType == "PopUp") 
                {
                    if (main.nextPopUp == "Merch") { ShowMerchPopUp(); main.nextPopUp = "RemoveAds"; }
                    else if (main.nextPopUp == "RemoveAds") {ShowRemoveAdsPopUp(); main.nextPopUp = "Merch";}
                    main.nextAdType = "Ad";
                }
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
    }
    private void HandleShowResult(ShowResult result)
    {
        gameManager.ContinueStartGame();
    }

    public void ShowMerchPopUp() 
    {
        merchPopUp.Invoke();
    }

    public void ShowRemoveAdsPopUp() 
    {
        removeAdsPopUp.Invoke();
    }
}
