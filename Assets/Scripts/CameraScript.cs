using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraScript : MonoBehaviour
{
    private GameManager gameManager;
    private Transform player;
    public float xOffset;
    public float yOffset;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        float x = player.position.x + xOffset;
        float y = 0;

        if (player.position.y > yOffset)
        {
            y = player.position.y - yOffset;
        }

        transform.position = new Vector3(x, y, -10);
    }

}
