using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdParent : MonoBehaviour
{
    public GameObject bird_child;
    
    void Awake()
    {
        bird_child = transform.GetChild(0).gameObject;
    }

    public void OnEnable()
    {
        bird_child.gameObject.SetActive(false);
        bird_child.gameObject.SetActive(true);
    }
}
