using UnityEngine;
using System.Linq;

public class UI_SettingsMusic : MonoBehaviour
{
    Main main;
    LocalSoundManager localSoundManager;
    private string[] same_music_scenes = new string[4] {"UI_ChooseLevel", "UI_About", "UI_Credits", "UI_Settings"};


    public void UnPause() {
        if (!same_music_scenes.Contains(main.actual_last_scene)) 
        {
            localSoundManager.UnPause("NationalAnthem");
        }
    }

    public void Pause() {
        localSoundManager.Pause("NationalAnthem");
    }
}

