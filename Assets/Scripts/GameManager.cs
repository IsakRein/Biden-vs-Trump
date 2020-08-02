using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Environment environment;
    public LevelManager levelManager;

    public float verticalSize;
    public float horizontalSize;
    
    public GameObject level;
  
    public bool customStartBool = true;

    void Start()
    {
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
        player.gameObject.SetActive(true);
        player.StartGame();
        levelManager.StartGame();
        
        level.SetActive(false);
        level.SetActive(true);

        environment.StartGame();
    }

    public void PauseGame() 
    {
        player.PauseGame();
        environment.PauseGame();
        levelManager.PauseGame();
    }

    public void ResumeGame() 
    {
        player.ResumeGame();
        environment.ResumeGame();
        levelManager.ResumeGame();
    }

    public void Death()
    {
        player.Death();
        environment.Death();
        levelManager.Death();

        Invoke("StartGame", 1);
    }
}



