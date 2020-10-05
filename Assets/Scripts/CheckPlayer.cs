using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckPlayer : MonoBehaviour
{
    public UnityEvent biden;
    public UnityEvent trump;

    void Start()
    {
        if (PlayerPrefs.GetString("Player") == "Joe Biden") {
            biden.Invoke();
        }
        else {
            trump.Invoke();
        }
    }
}
