using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurModifier : MonoBehaviour
{
    public float blurValue; 
    public Material blurMat;

    void Update()
    {
        blurMat.SetFloat("_Size", blurValue);
    }
}
