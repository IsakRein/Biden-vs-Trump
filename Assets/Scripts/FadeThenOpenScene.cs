using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeThenOpenScene : MonoBehaviour
{
    public string scene;
    private UI_OpenScene uI_OpenScene;

    private void Start() {
        uI_OpenScene = GetComponent<UI_OpenScene>();
    }

    public void setScene(string _scene) {
        scene = _scene;
    }

    public void openScene() {
        uI_OpenScene.OpenScene(scene);
    }
}
