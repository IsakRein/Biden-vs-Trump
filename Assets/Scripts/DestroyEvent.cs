using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEvent : MonoBehaviour
{
    public void disable() 
    {
        gameObject.SetActive(false);
    }

    public void destroy() 
    {
        Destroy(gameObject);
    }
}
