﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_SetPlayer : MonoBehaviour
{
    public string selected_player;
    public Main main;

    void Start()
    {
        if (PlayerPrefs.HasKey("Player")) 
        {
            SceneManager.LoadScene(sceneName: "UI_ChooseLevel");
        }
        main = GameObject.FindGameObjectWithTag("Main").GetComponent<Main>();
    }

    public void SetSelectedPlayer(string _selected_player) 
    {
        selected_player = _selected_player;
    }

    public void LoadSceneFromName(string scene_name) 
    {
        PlayerPrefs.SetString("Player", selected_player);

        if (selected_player == "Joe Biden") 
        {
            main.AddToValue("biden_player_count");
        }
        else 
        {
            main.AddToValue("trump_player_count");
        }

        SceneManager.LoadScene(sceneName: scene_name);
    }   
}
