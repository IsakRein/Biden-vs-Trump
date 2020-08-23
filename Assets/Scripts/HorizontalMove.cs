using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HorizontalMove : MonoBehaviour
{
    public RectTransform backgrounds;
    public RectTransform panels;
    public float duration;
    public AnimationCurve speed;

    public void startMoveLeft() {
        StartCoroutine(move(1));
    }

    public void startMoveRight() {
        StartCoroutine(move(-1));
    }

    IEnumerator move(int direction) 
    {
        Vector2 target_vector = new Vector2(Mathf.RoundToInt((backgrounds.offsetMin.x + 1920f*direction) / 1920) * 1920, 0);
        Vector2 start_vector = new Vector2(backgrounds.offsetMin.x, 0);
        
        float executedTime = 0;
        while (executedTime < duration)
        {
            executedTime += Time.deltaTime;
            
            Vector2 lerp_vector = Vector2.Lerp(start_vector, target_vector, speed.Evaluate(executedTime / duration));                
            backgrounds.offsetMin = lerp_vector;
            backgrounds.offsetMax = lerp_vector;
            panels.offsetMin = lerp_vector;
            panels.offsetMax = lerp_vector;

            yield return null;
        }

        backgrounds.offsetMin = target_vector;
        backgrounds.offsetMax = target_vector;
        panels.offsetMin = target_vector;
        panels.offsetMax = target_vector;
    }
}
