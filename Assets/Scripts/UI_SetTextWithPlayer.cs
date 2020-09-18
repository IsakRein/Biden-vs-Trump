using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
 

public class UI_SetTextWithPlayer : MonoBehaviour
{
    public UI_TextToSpriteIndex textObject;

    public string textBeforePlayer;
    public string textAfterPlayer;

    void Start() 
    {
        textObject.message = (textBeforePlayer + PlayerPrefs.GetString("Player") + textAfterPlayer);
        textObject.setObjectText();
    }
}
