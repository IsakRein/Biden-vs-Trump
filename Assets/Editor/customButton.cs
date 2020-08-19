using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TextManager))]
public class customButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TextManager textManager = (TextManager)target;
        if (GUILayout.Button("Generate Text"))
        {
            textManager.GenerateText();
        }
    }

}