using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[ExecuteInEditMode]
public class Text : MonoBehaviour
{
    public TextMeshProUGUI inlineText;
    public TextMeshProUGUI outlineText;

    public string text;

    void Update()
    {
        inlineText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        outlineText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        inlineText.SetText(text);      
        outlineText.SetText(text);      
    }
}
