using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform following;

    void Update()
    {
        transform.position = following.position;        
    }
}
