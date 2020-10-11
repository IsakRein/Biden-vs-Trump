using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class UI_ChooseLevelSound : MonoBehaviour
{
    private Main main;
    public LocalSoundManager localSoundManager;
    private string[] same_music_scenes = new string[3] {"UI_About", "UI_Credits", "UI_Settings"};

    private void Start() {
        main = FindObjectOfType<Main>().GetComponent<Main>();
        localSoundManager = GetComponent<LocalSoundManager>();

        Debug.Log(main.actual_last_scene);

        // if (!same_music_scenes.Contains(main.actual_last_scene)) 
        // {
        //     localSoundManager.Play("NationalAnthem");
        // }
        localSoundManager.IfNotPlayingPlay("NationalAnthem");
    }
}

