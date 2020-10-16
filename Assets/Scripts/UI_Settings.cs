using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI_Settings : MonoBehaviour
{
    public UnityEvent ads_removed = new UnityEvent(); 
    public UnityEvent ads_left = new UnityEvent(); 
    public List<Transform> transforms = new List<Transform>();

    private bool moved = false;

    void Start() {
        moved = false;
        updateUI();
    }

    public void updateUI() {
        if (PlayerPrefs.GetInt("remove_ads") == 1) 
        {
            ads_removed.Invoke();
            
            foreach (var item in transforms)
            {
                item.localPosition -= new Vector3(0f,200f,0f); 
            }
            moved = true;
        }
        else {
            ads_left.Invoke();
        }
    }
}
