using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Awake() {
        main = GameObject.FindGameObjectWithTag("Main").GetComponent<Main>();

        biden_players_text.updateFromDict(main.biden_player_count.ToString());
        trump_players_text.updateFromDict(main.trump_player_count.ToString());

        stats_level_1.setWidth(main.biden_level_1, main.trump_level_1);
        stats_level_2.setWidth(main.biden_level_2, main.trump_level_2);
        stats_level_3.setWidth(main.biden_level_3, main.trump_level_3);
        stats_level_4.setWidth(main.biden_level_4, main.trump_level_4);
        stats_level_5.setWidth(main.biden_level_5, main.trump_level_5);
        stats_level_6.setWidth(main.biden_level_6, main.trump_level_6);
    }
}
