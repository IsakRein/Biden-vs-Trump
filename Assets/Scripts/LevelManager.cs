using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private GameManager gameManager;

    private bool gameActive;

    public float levelScrollingSpeed = 10f;
    private float originalLevelScrollingSpeed;


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        originalLevelScrollingSpeed = levelScrollingSpeed;
    }
    
    void Update()
    {
        if (gameActive) {
            float x = transform.position.x - levelScrollingSpeed * Time.deltaTime;
            transform.position = new Vector2(x, 0);
        }
    }

    public void StartGame() 
    {
        gameActive = true;
        transform.position = new Vector2(0,0);
    }

    public void PauseGame()
    {
        gameActive = false;
    }

    public void ResumeGame()
    {
        gameActive = true;
    }
}
