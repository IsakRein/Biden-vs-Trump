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

    public void setMargin(float left, float right)
    {
        textObject.margin = new Vector4(left, 4.314225f, right, 3.137621f);
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
                else if (65 <= characterIndex && characterIndex <= 90 ) { newIndex = characterIndex - 65; }
                else {
                    switch (characterIndex)
                    {
                        case 37: newIndex = 36; break;
                        case 58: newIndex = 37; break;
                        case 45: newIndex = 38; break;
                        case 33: newIndex = 39; break;
                        case 43: newIndex = 40; break;
                        case 47: newIndex = 41; break;
                        default: newIndex = -1; break;
                    }
                }
                if (newIndex == -1) {
                    finalString += upperCaseInput[i];
                }
                else {
                    finalString += "<sprite index=" + newIndex + ">";
                }
            }    
            
        }
        return finalString;
    }
}
