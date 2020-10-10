using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomEvent : MonoBehaviour
{
    public UnityEvent customEvent = new UnityEvent();

    public UnityEvent customEvent2 = new UnityEvent();

    public void InvokeEvent1() {
        customEvent.Invoke();
    }

    public void InvokeEvent2() {
        customEvent2.Invoke();
    }
}
