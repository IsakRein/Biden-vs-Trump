using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public float levelScrollingSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        float x = transform.position.x - levelScrollingSpeed * Time.deltaTime;
        transform.position = new Vector2(x, 0);
    }
}
