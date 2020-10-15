using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{    
    public Player player;
    public Environment environment;
    public LevelManager levelManager;
    public AdManager adManager;

    public CameraScript cameraScript;

    public float verticalSize;
    public float horizontalSize;
    
    public GameObject level;
    
    public static Vector3 start_position;

    public bool customStartBool = true;
    public bool gameActive = true;

    public UnityEvent game_over;
    public UnityEvent game_won_event;
    
    public int percentage;
    public int previous_highscore;
    public float previous_highscore_x;

    public string current_level;
    public string current_level_key;
    public string current_last_x_key;
    public string current_last_y_key;

    public UI_GameWon uI_GameWon;
    public LocalSoundManager localSoundManager;


    private void Awake() {
        localSoundManager = FindObjectOfType<LocalSoundManager>();
    }

    void Start()
    {
        current_level_key = current_level + "_score";
        current_last_x_key = current_level + "_last_x";
        current_last_y_key = current_level + "_last_y";

        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();

        Application.targetFrameRate = 300;

        verticalSize = (float) (Camera.main.orthographicSize * 2.0);
        horizontalSize = verticalSize * Screen.width / Screen.height;

        environment.CustomStart();

        environment.StartGame();

        if (customStartBool) 
        {
            StartGame();
        }

        
    }

    public void StartGame() 
    {
        player.gameObject.SetActive(false);
        if (!adManager.PlayAd()) 
        {
            ContinueStartGame();
        }
    }

    public void ContinueStartGame() 
    {
        cameraScript.transform.position = new Vector3(0,0,0);

        player.gameObject.SetActive(true);
        player.StartGame();
        levelManager.StartGame();

        level.SetActive(false);
        level.SetActive(true);

        cameraScript.followPlayer = true;

        gameActive = true;
        //environment.StartGame();
        localSoundManager.Play(current_level);
    }

    public void PauseGame() 
    {
        player.PauseGame();
        environment.PauseGame();
        levelManager.PauseGame();
        gameActive = false;
        localSoundManager.Pause(current_level);
    }

    public void ResumeGame() 
    {
        player.ResumeGame();
        environment.ResumeGame();
        levelManager.ResumeGame();
        gameActive = true;
        localSoundManager.UnPause(current_level);
    }

    public void Death()
    {
        player.Death();
        environment.Death();
        levelManager.Death();

        if (gameActive) {
            game_over.Invoke();
            localSoundManager.Stop(current_level);
            localSoundManager.Play("Death");
        }

        gameActive = false;   
        
        
        
    }

    public void GameWon()
    {
        uI_GameWon.gameObject.SetActive(true);
        uI_GameWon.customStart();
        game_won_event.Invoke();
        gameActive = false;
        player.GameWon();
        cameraScript.followPlayer = false;
        environment.Death();
        levelManager.Death();
        
        localSoundManager.Play("Applause");
        localSoundManager.Play("PowerUp");
    }

    private void OnDestroy() {
        localSoundManager.Stop(current_level);
        localSoundManager.Stop("Applause");
    }
}



