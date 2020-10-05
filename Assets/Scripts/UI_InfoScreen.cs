using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UI_InfoScreen : MonoBehaviour
{
    public List<string> active_settings = new List<string>();

    public List<GameObject> text_objects = new List<GameObject>();
    public int current_active = -1;

    void Start()
    {
        ChangeUI();
    }

    public void ChangeUI() 
    {
        if (current_active < active_settings.Count - 1) 
        {
            current_active += 1;
        }

        for (int i = 0; i < text_objects.Count; i++)
        {
            text_objects[i].SetActive(num_to_bool(active_settings[current_active][i]));
        }  
    }

    bool num_to_bool(char num) {
        if (num == '1') { return true; }
        else { return false; }
    }
}
