using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AnimateCamera : MonoBehaviour
{
    public bool moveCamera;
    public Vector3 camera_position;
    public Transform camera;

    void Update()
    {
        if (moveCamera) {
            camera.position = camera_position;
        }
    }
}
