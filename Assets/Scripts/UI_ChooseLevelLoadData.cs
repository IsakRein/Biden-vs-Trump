using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase.Extensions;

public class UI_ChooseLevelLoadData : MonoBehaviour
{
    private Main main;
    public UI_TextToSpriteIndex biden_players_text;
    public UI_TextToSpriteIndex trump_players_text;
    public UI_Stats stats_level_1;
    public UI_Stats stats_level_2;
    public UI_Stats stats_level_3;
    public UI_Stats stats_level_4;
    public UI_Stats stats_level_5;
    public UI_Stats stats_level_6;

    public bool success;
    public bool loaded;

    void Awake() {
        main = GameObject.FindGameObjectWithTag("Main").GetComponent<Main>();

        writeText();

        if (main.actual_last_scene != "- Main -" && main.actual_last_scene != "UI_ChooseFighter") 
        {
            LoadData();
        }
    }

    void Update()
    {
        if (loaded) {
            Debug.Log("Downloaded data 2");

            if (success)
            {
                writeText();
                loaded = false;
            }
            loaded = false;
            success = false;
        }
    }

    void writeText() {
        biden_players_text.updateFromDict(main.biden_player_count.ToString());
        trump_players_text.updateFromDict(main.trump_player_count.ToString());

        stats_level_1.setWidth(main.biden_level_1, main.trump_level_1);
        stats_level_2.setWidth(main.biden_level_2, main.trump_level_2);
        stats_level_3.setWidth(main.biden_level_3, main.trump_level_3);
        stats_level_4.setWidth(main.biden_level_4, main.trump_level_4);
        stats_level_5.setWidth(main.biden_level_5, main.trump_level_5);
        stats_level_6.setWidth(main.biden_level_6, main.trump_level_6);
    }

    public void LoadData()
    {
        var task1 = main.reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                success = false;
                loaded = true;
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

                success = true;
                loaded = true;
            }
        });
    }
}
