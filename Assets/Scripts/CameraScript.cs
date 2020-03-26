using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private GameManager gameManager;
    private Transform player;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (player.position.y > 30) {
            transform.position = new Vector3(0, player.position.y-30, -10);
        }
    }

}
