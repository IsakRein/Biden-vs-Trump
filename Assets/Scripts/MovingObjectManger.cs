using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectManger : MonoBehaviour
{
    public Component[] animators;
    void Start()
    {
        animators = GetComponentsInChildren(typeof(Animator));
    }

    public void Pause() {
        foreach (Animator animator in animators) {
            animator.enabled = false;
        }
    }
    public void Resume() {
        foreach (Animator animator in animators) {
            animator.enabled = true;
        }
    }
}
