using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using VoxelBusters.NativePlugins;

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

    public void OpenLeaderboardUI(string level) {
        // bool _isAuthenticated = NPBinding.GameServices.LocalUser.IsAuthenticated;
        // if (!_isAuthenticated) {
        //     NPBinding.GameServices.LocalUser.Authenticate((bool _success, string _error)=>{
        //         if (_success)
        //         {
        //             Debug.Log("Sign-In Successfully");
        //             Debug.Log("Local User Details : " + NPBinding.GameServices.LocalUser.ToString());
        //         }
        //         else
        //         {
        //             Debug.Log("Sign-In Failed with error " + _error);
        //         }
        //     });
        // }
        
        // NPBinding.GameServices.ShowLeaderboardUIWithGlobalID(level, eLeaderboardTimeScope.ALL_TIME, (string _error)=>{
        //     Debug.Log("Leaderboard view dismissed.");
        // });
    }
}   
