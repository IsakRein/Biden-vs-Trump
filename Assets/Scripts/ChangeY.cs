using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeY : MonoBehaviour
{
    public float y = 0;

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector2(transform.localPosition.x, y);
    }
}
