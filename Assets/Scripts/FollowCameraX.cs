using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraX : MonoBehaviour
{
	public Transform camera_transform; 

    void Update()
    {
        transform.position = new Vector3(transform.position.x, 0,0);
    }
}
