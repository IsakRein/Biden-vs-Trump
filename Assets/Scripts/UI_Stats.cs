using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class UI_Stats : MonoBehaviour
{
    public int biden_count;
    public int trump_count;

    public float width = 10.0f;

    public Transform stats_BL;
    public Transform stats_BM;
    public Transform stats_BR;
    public Transform stats_MM;
    public Transform stats_RL;
    public Transform stats_RM;
    public Transform stats_RR;

    public UI_TextToSpriteIndex biden_count_text;
    public UI_TextToSpriteIndex trump_count_text;
    public UI_TextToSpriteIndex stats_info_text;


    public float side_piece_width;
    public float mid_piece_width;
    public float mid_mid_piece_width;

    
    /* void Update()
    {
        setWidth(debug_biden_count, debug_trump_count);
    } */

    public void setWidth(int _biden_count, int _trump_count) 
    {
        biden_count = _biden_count;
        trump_count = _trump_count;

        int total = biden_count + trump_count;
        float percentage = 0.5f;

        if (total != 0) 
        {  
            percentage = (float)biden_count/(float)total;
        }

        if (biden_count > trump_count) { stats_info_text.updateFromDict("Biden is leading"); }
        else if (biden_count < trump_count) { stats_info_text.updateFromDict("Trump is leading"); }
        else { stats_info_text.updateFromDict("Draw"); }
        biden_count_text.updateFromDict(biden_count.ToString());
        trump_count_text.updateFromDict(trump_count.ToString());

        stats_BL.gameObject.SetActive(percentage != 0.0f);
        stats_RL.gameObject.SetActive(percentage == 0.0f);
        stats_BM.gameObject.SetActive(percentage != 0.0f);
        stats_MM.gameObject.SetActive(percentage != 0.0f && percentage != 1.0f);
        stats_RM.gameObject.SetActive(percentage != 1.0f);
        stats_BR.gameObject.SetActive(percentage == 1.0f);
        stats_RR.gameObject.SetActive(percentage != 1.0f);

        float l_pos = -(width / 2) + (side_piece_width / 2);
        float r_pos = (width / 2) - (side_piece_width / 2);
        float mid_piece_space = width - (2 * side_piece_width) - (mid_mid_piece_width);
        float mid_mid_piece_x = (mid_piece_space * percentage) - (mid_piece_space / 2);
        float b_left_edge  = -(width / 2) + (side_piece_width) + mid_piece_width / 2;
        float b_right_edge = mid_mid_piece_x + (mid_mid_piece_width / 2) - mid_piece_width / 2;
        float r_left_edge  = mid_mid_piece_x - (mid_mid_piece_width / 2) + mid_piece_width / 2;
        float r_right_edge = (width / 2) - (side_piece_width) - mid_piece_width / 2;

        biden_count_text.setMargin(0, -mid_mid_piece_x);
        trump_count_text.setMargin(mid_mid_piece_x, 0);

        Debug.Log(mid_mid_piece_x); 


        stats_BL.localPosition = new Vector2(l_pos, 0);
        stats_RR.localPosition = new Vector2(r_pos, 0);
        stats_RL.localPosition = new Vector2(l_pos, 0);
        stats_BR.localPosition = new Vector2(r_pos, 0);

        stats_BM.localPosition = new Vector2((b_left_edge + b_right_edge) / 2, 0);    
        stats_RM.localPosition = new Vector2((r_left_edge + r_right_edge) / 2, 0);

        stats_BM.localScale = new Vector2(Math.Abs(b_left_edge - b_right_edge) + mid_piece_width, 1);
        stats_RM.localScale = new Vector2(Math.Abs(r_left_edge - r_right_edge) + mid_piece_width, 1);

        stats_MM.localPosition = new Vector2(mid_mid_piece_x, 0);
    }
}

