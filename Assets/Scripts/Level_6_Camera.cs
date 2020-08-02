using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_6_Camera : MonoBehaviour
{
    public List<GameObject> gameObjects = new List<GameObject>();

    public CameraScript cameraScript;
    public GameManager gameManager;
    public Vector3 custom_position;

    public bool game_started = false;

    void Update()
    {
        if (game_started == false) {
            transform.position = custom_position;
        }
    }

    void ActivateGame() 
    {
        foreach (var item in gameObjects)
        {
            item.SetActive(true);  
        }
        cameraScript.enabled = true;
        gameManager.StartGame();

        game_started = true;
    }
}
