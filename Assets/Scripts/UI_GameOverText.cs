using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameOverText : MonoBehaviour
{
    public GameManager gameManager;
    public UI_TextToSpriteIndex percentage_text;
    public UI_TextToSpriteIndex bottom_text;

    void OnEnable()
    {
        percentage_text.updateFromDict(gameManager.percentage.ToString() + "%");


        if (gameManager.percentage >= gameManager.previous_highscore) {
            bottom_text.updateFromDict("new best");
        }   
        else {
            bottom_text.updateFromDict("best: " + PlayerPrefs.GetInt(gameManager.current_level_key).ToString() + "%");
        }
    }
}
