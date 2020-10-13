using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_OpenScene : MonoBehaviour
{
    public static string last_scene;
   
    string[] bad_scenes = new string[4] {"UI_About", "UI_Credits", "UI_InfoScreen", "UI_Settings"};

    Main main;

    private void Start() {
        main = FindObjectOfType<Main>().GetComponent<Main>();
    }

    public void OpenScene(string name) 
    {
        string currentScene = SceneManager.GetActiveScene().name;
        main = FindObjectOfType<Main>().GetComponent<Main>();
        
        main.actual_last_scene = currentScene;

        if (!bad_scenes.Contains(currentScene)) 
        { 
            last_scene = currentScene;
        }  
        
        if (name == "") 
        {
            SceneManager.LoadScene(sceneName: "Main");
        }
        else 
        {
            SceneManager.LoadScene(sceneName: name);
        }
        
    }

    public void GoBackSceneUnlessMain() {
        if (last_scene != "Main") {
            OpenScene(last_scene);
        }
    }

    public void GoBackScene() {
        OpenScene(last_scene);
    }

    public void OpenURL(string url) {
        Application.OpenURL(url);
    }
}
