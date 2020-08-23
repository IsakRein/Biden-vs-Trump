using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_OpenScene : MonoBehaviour
{
    public void OpenScene(string name) 
    {
        SceneManager.LoadScene(sceneName: name);
    }
}
