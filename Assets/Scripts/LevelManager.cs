using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private bool gameActive;
    public Component[] movingObjectMangers;
    public void StartGame() 
    {
        gameActive = true;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        movingObjectMangers = GetComponentsInChildren(typeof(MovingObjectManger));
    }

    public void PauseGame()
    {
        gameActive = false;
        foreach (MovingObjectManger movingObjectManger in movingObjectMangers) {
            movingObjectManger.Pause();
        }
    }

    public void ResumeGame()
    {
        gameActive = true;
        foreach (MovingObjectManger movingObjectManger in movingObjectMangers) {
            movingObjectManger.Resume();
        }
    }

    public void Death()
    {
        gameActive = false;
    }
}
