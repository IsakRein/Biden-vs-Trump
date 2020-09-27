using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_OpenScene : MonoBehaviour
{
    public static string last_scene;

    public void OpenScene(string name) 
    {
        last_scene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName: name);
    }

    public void GoBackScene() {
        OpenScene(last_scene);
    }
}
