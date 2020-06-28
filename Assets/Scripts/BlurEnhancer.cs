using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurEnhancer : MonoBehaviour
{
    public Material blur;
    
    public float min_blur;
    public float max_blur;
    public float transition_time;

    private float current_blur;

    void OnEnable()
    {
        current_blur = min_blur;
        blur.SetFloat("_Size", current_blur);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player") {
            StartCoroutine(AddBlur());
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player") {
            StartCoroutine(SubtractBlur());
        }
    }

    IEnumerator AddBlur() 
    {
        while (current_blur < max_blur) {
            current_blur += (max_blur - min_blur) * (Time.deltaTime/transition_time);
            blur.SetFloat("_Size", current_blur);

            yield return null;
        }
    }

    IEnumerator SubtractBlur() 
    {
        while (current_blur > min_blur) {
            current_blur -= (max_blur - min_blur) * (Time.deltaTime/transition_time);
            blur.SetFloat("_Size", current_blur);

            yield return null;
        }
    }
}
