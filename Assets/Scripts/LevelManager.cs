using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private GameManager gameManager;
    private bool gameActive;

    public float levelScrollingSpeed;


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void StartGame() 
    {
        gameActive = true;
        //transform.position = new Vector2(0,0);
    }

    public void PauseGame()
    {
        gameActive = false;
    }

    public void ResumeGame()
    {
        gameActive = true;
    }

    public void Death()
    {
        gameActive = false;
    }
}
