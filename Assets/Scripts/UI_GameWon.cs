using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameWon : MonoBehaviour
{
    public Main main;
    public GameManager gameManager;

    public UI_Stats uI_Stats;

    public GameObject biden_plus;
    public GameObject trump_plus;

    private void Awake() 
    {
        main = GameObject.FindGameObjectWithTag("Main").GetComponent<Main>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();   
    }

    public void customStart() {
        biden_plus.SetActive(false);
        trump_plus.SetActive(false);

        Debug.Log(gameManager.current_level_key);
        if (PlayerPrefs.GetInt(gameManager.current_level_key) < 100) {
            //First time
            Debug.Log("First time " + PlayerPrefs.GetString("Player"));
            string db_key = "";
            if (PlayerPrefs.GetString("Player") == "Joe Biden") {
                biden_plus.SetActive(true);
                db_key = "biden_" + gameManager.current_level;
            }
            if (PlayerPrefs.GetString("Player") == "Donald Trump") {
                trump_plus.SetActive(true);
                db_key = "trump_" + gameManager.current_level;
            }

            main.AddToValue(db_key);
        }

        else {
            Debug.Log("Not first time " + PlayerPrefs.GetString("Player"));
        }

        if (gameManager.current_level == "level_1") {
            uI_Stats.setWidth(main.biden_level_1, main.trump_level_1);
        }
        if (gameManager.current_level == "level_2") {
            uI_Stats.setWidth(main.biden_level_2, main.trump_level_2);
        }
        if (gameManager.current_level == "level_3") {
            uI_Stats.setWidth(main.biden_level_3, main.trump_level_3);
        }
        if (gameManager.current_level == "level_4") {
            uI_Stats.setWidth(main.biden_level_4, main.trump_level_4);
        }
        if (gameManager.current_level == "level_5") {
            uI_Stats.setWidth(main.biden_level_5, main.trump_level_5);
        }
        if (gameManager.current_level == "level_6") {
            uI_Stats.setWidth(main.biden_level_6, main.trump_level_6);
        }
    }    
}
