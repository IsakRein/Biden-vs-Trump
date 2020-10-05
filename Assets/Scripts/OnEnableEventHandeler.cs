using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEnableEventHandeler : MonoBehaviour
{
    public UnityEvent onEnable = new UnityEvent();
    
    void OnEnable()
    {
        onEnable.Invoke();
    }

    public void ReactivateAllChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
            child.gameObject.SetActive(true);
        }
    }
}
