using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_TextToSpriteIndex : MonoBehaviour
{
    public TextMeshProUGUI textObject;
    public string message;

    void Awake()
    {
        textObject = GetComponent<TextMeshProUGUI>();
        setObjectText();
    }

    public void updateFromDict(string newMessage) 
    {
        message = (string)newMessage;
        setObjectText();
    }

    public void setObjectText() 
    {
        textObject.SetText(convertToSpriteIndex(message));
    }

    private string convertToSpriteIndex(string input) 
    {
        string upperCaseInput = input.ToUpper();
        string finalString = "";
        for (int i = 0; i < upperCaseInput.Length; i++) 
        {
            int characterIndex = (int)(upperCaseInput[i]);
            int newIndex = 0;
            if (characterIndex == 32) 
            {
                finalString += " ";
            }
            
            else {
                if (48 <= characterIndex && characterIndex <= 57) { newIndex = characterIndex - 22; }
                else { newIndex = characterIndex - 65; }
                finalString += "<sprite index=" + newIndex + ">";
            }    
            
        }
        return finalString;
    }
}
