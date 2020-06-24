using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private GameManager gameManager;
    private Transform player;
    private Player playerScript;
    public float xOffset;
    public float yOffset;
    private float x_position;
    float verticalSize;
    float horizontalSize;
    
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player").transform;
        playerScript = player.gameObject.GetComponent<Player>();
	
		verticalSize = (float) (Camera.main.orthographicSize * 2.0);
		horizontalSize = verticalSize * Screen.width / Screen.height;
        x_position = -horizontalSize/2 + xOffset;
    }

    void Update()
    {
        float x = player.position.x - x_position;
        float y = 0;

        verticalSize = (float) (Camera.main.orthographicSize * 2.0);
		horizontalSize = verticalSize * Screen.width / Screen.height;
        x_position = -horizontalSize/2 + xOffset;

        if (player.position.y > yOffset && !playerScript.jetpack_active)
        {
            y = player.position.y - yOffset;
        }

        transform.position = new Vector3(x, y, -10);
    }

}
