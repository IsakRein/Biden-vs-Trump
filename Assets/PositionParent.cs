using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionParent : MonoBehaviour
{ 
    void Start()
    {
        Transform child = transform.GetChild(0);
        Vector3 childPos;
        childPos = child.localPosition;
        transform.position += childPos;

        child.position = new Vector3(0,0,0);
    }
}
