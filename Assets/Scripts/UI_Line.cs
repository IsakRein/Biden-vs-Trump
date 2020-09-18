using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UI_Line : MonoBehaviour
{
    [Range(0, 1)]
    public float input_percentage = 0.0f;
    public float width = 10.0f;

    public Transform line_left;
    public Transform line_middle;
    public Transform line_right;

    public int side_width;

    void Update()
    {
        setWidth(input_percentage);
    }

    void setWidth(float percentage) 
    {
        float middleWidth = width - 2 * side_width;
        float percentageWidth = width - side_width;
        float lower_lim = 1;
        float upper_lim = percentageWidth - 1;
        float percentageValue = Mathf.FloorToInt(percentage*percentageWidth);

        float total_width = percentage * percentageWidth;
        float new_middle_scale = total_width - lower_lim;
        if (upper_lim < total_width) {new_middle_scale = upper_lim - 1;}
        float new_middle_pos = -(middleWidth / 2) + (new_middle_scale / 2);
        float new_right_pos = new_middle_pos + (new_middle_scale / 2) + 1;

        line_left.localPosition = new Vector2(-(middleWidth/2)-1, 0);
        line_middle.localScale = new Vector2(new_middle_scale, 1);
        line_middle.localPosition = new Vector2(new_middle_pos, 0);
        line_right.localPosition = new Vector2(new_right_pos, 0);

        if (lower_lim <= percentageValue) {line_left.gameObject.SetActive(true);line_middle.gameObject.SetActive(true);line_right.gameObject.SetActive(true);}
        else {line_left.gameObject.SetActive(false);line_middle.gameObject.SetActive(false);line_right.gameObject.SetActive(false);}
    }

    void setPercentage () {



    }

}
