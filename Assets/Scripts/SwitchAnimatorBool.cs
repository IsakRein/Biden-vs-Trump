using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAnimatorBool : MonoBehaviour
{
    public Animator animator;
    public string boolname;
    private bool value;

    public void ToggleBool(bool val)
    {
        animator.SetBool(boolname, val);
    }
}
