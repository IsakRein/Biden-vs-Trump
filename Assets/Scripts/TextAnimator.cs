using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAnimator : MonoBehaviour
{
    public List<Animation> animations = new List<Animation>();

    void OnEnable()
    {
        foreach (Transform child in transform) 
        {
            animations.Add(child.GetComponent<Animation>());
        }

        StartCoroutine(trigger_anim());
    }


    IEnumerator trigger_anim() 
    {
        int childNum = 0;
        while (true) 
        {
            if (childNum >= transform.childCount) { childNum = 0; }

            animations[childNum].Play();
            childNum += 1;

            yield return new WaitForSeconds(0.1f);
        }
    }
}
