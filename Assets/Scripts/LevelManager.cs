using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private bool gameActive;

    public void StartGame() 
    {
        gameActive = true;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
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
