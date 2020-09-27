using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ChooseLevel6 : MonoBehaviour
{

    public GameObject play_button;
    public Image play_button_image;
    public Button play_button_button;

    public GameObject play_text;
    public GameObject info_text;

    public ButtonMoveText buttonMoveText;

    public bool level_1_won;
    public bool level_2_won;
    public bool level_3_won;
    public bool level_4_won;
    public bool level_5_won;
    public bool level_6_won;

    void Start()
    {
        level_1_won = PlayerPrefs.GetFloat("level_1_score") >= 100.0f;       
        level_2_won = PlayerPrefs.GetFloat("level_2_score") >= 100.0f;       
        level_3_won = PlayerPrefs.GetFloat("level_3_score") >= 100.0f;       
        level_4_won = PlayerPrefs.GetFloat("level_4_score") >= 100.0f;       
        level_5_won = PlayerPrefs.GetFloat("level_5_score") >= 100.0f;       
        level_6_won = PlayerPrefs.GetFloat("level_6_score") >= 100.0f;

        play_button_button = play_button.GetComponent<Button>();
        buttonMoveText = play_button.GetComponent<ButtonMoveText>();

        SpriteState spriteState = new SpriteState();
        spriteState = play_button_button.spriteState;

        if (level_1_won && level_2_won && level_3_won && level_4_won && level_5_won) 
        {
            play_button_button.interactable = true;

            play_text.SetActive(true);
            info_text.SetActive(false);

            buttonMoveText.enabled = true;
            info_text.SetActive(false);
        }
        else {
            play_button_button.interactable = false;

            play_text.SetActive(false);
            info_text.SetActive(true);

            buttonMoveText.enabled = false;
            info_text.SetActive(true);
        }

    }
}
