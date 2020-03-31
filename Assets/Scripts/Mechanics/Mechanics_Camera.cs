using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanics_Camera : MonoBehaviour
{
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x+4, player.transform.position.y+1, -10);
    }
}
