using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DatabaseManager : MonoBehaviour
{
    public Main main;
    public UI_OpenScene openScene;
    DatabaseReference reference;

    void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError($"Failed to initialize firebase with {task.Exception}");
                main.success = false;
                main.loaded = true;
            }
            
            LoadData();
        });
    }

    public void LoadData()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://bidenvstrump-56b45.firebaseio.com/");
        FirebaseApp.DefaultInstance.SetEditorP12FileName("bidenvstrump-56b45-4572a189b33e.p12");
        FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail("unity-762@bidenvstrump-56b45.iam.gserviceaccount.com");
        FirebaseApp.DefaultInstance.SetEditorP12Password("notasecret");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        main.reference = reference;

        var task1 = reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                main.success = false;
                main.loaded = true;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                main.biden_level_1 = int.Parse(snapshot.Child("biden_level_1").Value.ToString());
                main.biden_level_2 = int.Parse(snapshot.Child("biden_level_2").Value.ToString());
                main.biden_level_3 = int.Parse(snapshot.Child("biden_level_3").Value.ToString());
                main.biden_level_4 = int.Parse(snapshot.Child("biden_level_4").Value.ToString());
                main.biden_level_5 = int.Parse(snapshot.Child("biden_level_5").Value.ToString());
                main.biden_level_6 = int.Parse(snapshot.Child("biden_level_6").Value.ToString());
                main.biden_player_count = int.Parse(snapshot.Child("biden_player_count").Value.ToString());
                main.trump_level_1 = int.Parse(snapshot.Child("trump_level_1").Value.ToString());
                main.trump_level_2 = int.Parse(snapshot.Child("trump_level_2").Value.ToString());
                main.trump_level_3 = int.Parse(snapshot.Child("trump_level_3").Value.ToString());
                main.trump_level_4 = int.Parse(snapshot.Child("trump_level_4").Value.ToString());
                main.trump_level_5 = int.Parse(snapshot.Child("trump_level_5").Value.ToString());
                main.trump_level_6 = int.Parse(snapshot.Child("trump_level_6").Value.ToString());
                main.trump_player_count = int.Parse(snapshot.Child("trump_player_count").Value.ToString());

                main.success = true;
                main.loaded = true;
            }
        });
    }

    void Update()
    {
        if (main.loaded)
        {
            Debug.Log("Downloaded data");
            if (!PlayerPrefs.HasKey("trump_player_count")) { createPlayerPrefs(); }

            if (main.success)
            {                
                setPlayerPrefs();
            }
            else
            {
                loadPlayerPrefs();
            }
            if (PlayerPrefs.HasKey("Player")) { openScene.OpenScene("UI_ChooseLevel"); }
            else {openScene.OpenScene("UI_ChooseFighter"); }
        }
    }

    void setPlayerPrefs()
    {
        PlayerPrefs.SetInt("biden_level_1", main.biden_level_1);
        PlayerPrefs.SetInt("biden_level_2", main.biden_level_2);
        PlayerPrefs.SetInt("biden_level_3", main.biden_level_3);
        PlayerPrefs.SetInt("biden_level_4", main.biden_level_4);
        PlayerPrefs.SetInt("biden_level_5", main.biden_level_5);
        PlayerPrefs.SetInt("biden_level_6", main.biden_level_6);
        PlayerPrefs.SetInt("biden_player_count", main.biden_player_count);
        PlayerPrefs.SetInt("trump_level_1", main.trump_level_1);
        PlayerPrefs.SetInt("trump_level_2", main.trump_level_2);
        PlayerPrefs.SetInt("trump_level_3", main.trump_level_3);
        PlayerPrefs.SetInt("trump_level_4", main.trump_level_4);
        PlayerPrefs.SetInt("trump_level_5", main.trump_level_5);
        PlayerPrefs.SetInt("trump_level_6", main.trump_level_6);
        PlayerPrefs.SetInt("trump_player_count", main.trump_player_count);
    }

    void loadPlayerPrefs()
    {
        main.biden_level_1 = PlayerPrefs.GetInt("biden_level_1");
        main.biden_level_2 = PlayerPrefs.GetInt("biden_level_2");
        main.biden_level_3 = PlayerPrefs.GetInt("biden_level_3");
        main.biden_level_4 = PlayerPrefs.GetInt("biden_level_4");
        main.biden_level_5 = PlayerPrefs.GetInt("biden_level_5");
        main.biden_level_6 = PlayerPrefs.GetInt("biden_level_6");
        main.biden_player_count = PlayerPrefs.GetInt("biden_player_count");
        main.trump_level_1 = PlayerPrefs.GetInt("trump_level_1");
        main.trump_level_2 = PlayerPrefs.GetInt("trump_level_2");
        main.trump_level_3 = PlayerPrefs.GetInt("trump_level_3");
        main.trump_level_4 = PlayerPrefs.GetInt("trump_level_4");
        main.trump_level_5 = PlayerPrefs.GetInt("trump_level_5");
        main.trump_level_6 = PlayerPrefs.GetInt("trump_level_6");
        main.trump_player_count = PlayerPrefs.GetInt("trump_player_count");
    }

    void createPlayerPrefs()
    {
		//General
		if (!PlayerPrefs.HasKey("last_scene")) { PlayerPrefs.SetString("last_scene", "Level 3"); }

        //Settings
        if (!PlayerPrefs.HasKey("settings_sound")) { PlayerPrefs.SetInt("settings_sound", 1); }
        if (!PlayerPrefs.HasKey("settings_music")) { PlayerPrefs.SetInt("settings_music", 1); }
        if (!PlayerPrefs.HasKey("settings_vibration")) { PlayerPrefs.SetInt("settings_vibration", 1); }
        if (!PlayerPrefs.HasKey("remove_ads")) { PlayerPrefs.SetInt("remove_ads", 0); }

        //Scores
        if (!PlayerPrefs.HasKey("level_1_score")) { PlayerPrefs.SetInt("level_1_score", 0); }
        if (!PlayerPrefs.HasKey("level_2_score")) { PlayerPrefs.SetInt("level_2_score", 0); }
        if (!PlayerPrefs.HasKey("level_3_score")) { PlayerPrefs.SetInt("level_3_score", 0); }
        if (!PlayerPrefs.HasKey("level_4_score")) { PlayerPrefs.SetInt("level_4_score", 0); }
        if (!PlayerPrefs.HasKey("level_5_score")) { PlayerPrefs.SetInt("level_5_score", 0); }
        if (!PlayerPrefs.HasKey("level_6_score")) { PlayerPrefs.SetInt("level_6_score", 0); }

        if (!PlayerPrefs.HasKey("level_1_score_x")) { PlayerPrefs.SetFloat("level_1_score_x", 0); }
        if (!PlayerPrefs.HasKey("level_2_score_x")) { PlayerPrefs.SetFloat("level_2_score_x", 0); }
        if (!PlayerPrefs.HasKey("level_3_score_x")) { PlayerPrefs.SetFloat("level_3_score_x", 0); }
        if (!PlayerPrefs.HasKey("level_4_score_x")) { PlayerPrefs.SetFloat("level_4_score_x", 0); }
        if (!PlayerPrefs.HasKey("level_5_score_x")) { PlayerPrefs.SetFloat("level_5_score_x", 0); }
        if (!PlayerPrefs.HasKey("level_6_score_x")) { PlayerPrefs.SetFloat("level_6_score_x", 0); }


        // //Scores
        // if (!PlayerPrefs.HasKey("level_1_last_x")) { PlayerPrefs.SetFloat("level_1_last_x", 0f); }
        // if (!PlayerPrefs.HasKey("level_1_last_y")) { PlayerPrefs.SetFloat("level_1_last_y", 0f); }
        // if (!PlayerPrefs.HasKey("level_2_last_x")) { PlayerPrefs.SetFloat("level_2_last_x", 0f); }
        // if (!PlayerPrefs.HasKey("level_2_last_y")) { PlayerPrefs.SetFloat("level_2_last_y", 0f); }
        // if (!PlayerPrefs.HasKey("level_3_last_x")) { PlayerPrefs.SetFloat("level_3_last_x", 0f); }
        // if (!PlayerPrefs.HasKey("level_3_last_y")) { PlayerPrefs.SetFloat("level_3_last_y", 0f); }
        // if (!PlayerPrefs.HasKey("level_4_last_x")) { PlayerPrefs.SetFloat("level_4_last_x", 0f); }
        // if (!PlayerPrefs.HasKey("level_4_last_y")) { PlayerPrefs.SetFloat("level_4_last_y", 0f); }
        // if (!PlayerPrefs.HasKey("level_5_last_x")) { PlayerPrefs.SetFloat("level_5_last_x", 0f); }
        // if (!PlayerPrefs.HasKey("level_5_last_y")) { PlayerPrefs.SetFloat("level_5_last_y", 0f); }
        // if (!PlayerPrefs.HasKey("level_6_last_x")) { PlayerPrefs.SetFloat("level_6_last_x", 0f); }
        // if (!PlayerPrefs.HasKey("level_6_last_y")) { PlayerPrefs.SetFloat("level_6_last_y", 0f); }

        //Stats
        if (!PlayerPrefs.HasKey("biden_level_1")) { PlayerPrefs.SetInt("biden_level_1", 0); }
        if (!PlayerPrefs.HasKey("biden_level_2")) { PlayerPrefs.SetInt("biden_level_2", 0); }
        if (!PlayerPrefs.HasKey("biden_level_3")) { PlayerPrefs.SetInt("biden_level_3", 0); }
        if (!PlayerPrefs.HasKey("biden_level_4")) { PlayerPrefs.SetInt("biden_level_4", 0); }
        if (!PlayerPrefs.HasKey("biden_level_5")) { PlayerPrefs.SetInt("biden_level_5", 0); }
        if (!PlayerPrefs.HasKey("biden_level_6")) { PlayerPrefs.SetInt("biden_level_6", 0); }
        if (!PlayerPrefs.HasKey("biden_player_count")) { PlayerPrefs.SetInt("biden_player_count", 0); }
        if (!PlayerPrefs.HasKey("trump_level_1")) { PlayerPrefs.SetInt("trump_level_1", 0); }
        if (!PlayerPrefs.HasKey("trump_level_2")) { PlayerPrefs.SetInt("trump_level_2", 0); }
        if (!PlayerPrefs.HasKey("trump_level_3")) { PlayerPrefs.SetInt("trump_level_3", 0); }
        if (!PlayerPrefs.HasKey("trump_level_4")) { PlayerPrefs.SetInt("trump_level_4", 0); }
        if (!PlayerPrefs.HasKey("trump_level_5")) { PlayerPrefs.SetInt("trump_level_5", 0); }
        if (!PlayerPrefs.HasKey("trump_level_6")) { PlayerPrefs.SetInt("trump_level_6", 0); }
        if (!PlayerPrefs.HasKey("trump_player_count")) { PlayerPrefs.SetInt("trump_player_count", 0); }

        PlayerPrefs.Save();
    }
}