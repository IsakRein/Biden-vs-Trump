using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SetLevelSelectText : MonoBehaviour
{   
    public UI_TextToSpriteIndex level_1_text;
    public UI_TextToSpriteIndex level_2_text;
    public UI_TextToSpriteIndex level_3_text;
    public UI_TextToSpriteIndex level_4_text;
    public UI_TextToSpriteIndex level_5_text;
    public UI_TextToSpriteIndex level_6_text;

    void Start()
    {
        level_1_text.updateFromDict(PlayerPrefs.GetInt("level_1_score").ToString() + "%");
        level_2_text.updateFromDict(PlayerPrefs.GetInt("level_2_score").ToString() + "%");
        level_3_text.updateFromDict(PlayerPrefs.GetInt("level_3_score").ToString() + "%");
        level_4_text.updateFromDict(PlayerPrefs.GetInt("level_4_score").ToString() + "%");
        level_5_text.updateFromDict(PlayerPrefs.GetInt("level_5_score").ToString() + "%");
        level_6_text.updateFromDict(PlayerPrefs.GetInt("level_6_score").ToString() + "%");
    }
}
