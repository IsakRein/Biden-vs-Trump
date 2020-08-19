using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    public string text;
    public List<Sprite> font1 = new List<Sprite>();
    public GameObject char_prefab;
    public float space_width;
    public float standard_width = 14;

    public void GenerateText()
    {
        float standard_width = 14 / transform.localScale.x;

        while(transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        float total_width = 0;

        for (int i = 0; i < text.Length; i++)
        {
            char character = text.ToUpper()[i];
            Debug.Log((int)character);
        }

        for (int i = 0; i < text.Length; i++)
        {
            char character = text.ToUpper()[i];
            if (character == ' ') 
            {
                total_width += (space_width / transform.localScale.x) / standard_width;
            }
            else 
            {
                GameObject char_obj = GameObject.Instantiate(char_prefab, transform);
                Sprite char_sprite;
                
                if (48 <= (int)character && (int)character <= 57) 
                {
                    char_sprite = font1[((int)character) - 22];
                }
                else 
                {
                    char_sprite = font1[((int)character) - 65];
                }
                
                float char_width = ((char_sprite.rect.xMax - char_sprite.rect.xMin)-1) / transform.localScale.x;
                total_width += char_width/2;
                char_obj.GetComponent<SpriteRenderer>().sprite = char_sprite;
                char_obj.transform.localPosition = new Vector2(total_width / standard_width, 0);
                total_width += char_width/2;
            }
        }

        foreach (Transform child in transform) 
        {
            child.transform.localPosition = new Vector2(child.transform.localPosition.x - (total_width / (2 * standard_width)), 0);
        }
    }
}
