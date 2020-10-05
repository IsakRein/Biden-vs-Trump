using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEnterEvent : MonoBehaviour
{
    public UnityEvent onTriggerEnter = new UnityEvent();

    void OnTriggerEnter2D(Collider2D other)
    {
        onTriggerEnter.Invoke();
    }
}
