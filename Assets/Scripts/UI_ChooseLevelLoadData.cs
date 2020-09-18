using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ChooseLevelLoadData : MonoBehaviour
{
    private Main main;
    public UI_TextToSpriteIndex biden_players_text;
    public UI_TextToSpriteIndex trump_players_text;

    void Awake() {
        main = GameObject.FindGameObjectWithTag("Main").GetComponent<Main>();

        biden_players_text.updateFromDict(main.biden_player_count.ToString());
        trump_players_text.updateFromDict(main.trump_player_count.ToString());

    }
}
