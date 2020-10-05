using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_RemoveAds : MonoBehaviour
{
    public Main main;

    void Start()
    {
        main = GameObject.FindGameObjectWithTag("Main").GetComponent<Main>(); 
    }


    public void RemoveAds()
    {
        PlayerPrefs.SetInt("remove_ads", 1);
        main.updateSettings();   
    }
}
