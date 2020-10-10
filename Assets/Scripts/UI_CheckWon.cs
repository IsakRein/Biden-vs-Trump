using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CheckWon : MonoBehaviour
{
    public string key;

    public Sprite regular;
    public Sprite regular_clicked;
    public Sprite finished;
    public Sprite finished_clicked;

    Image image;
    Button button;


    void Start()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();

        SpriteState spriteState = new SpriteState();
        spriteState = button.spriteState;

        if (PlayerPrefs.GetInt(key) >= 100) 
        {
            image.sprite = finished;
            spriteState.pressedSprite = finished_clicked;
        }
        else {
            image.sprite = regular;
            spriteState.pressedSprite = regular_clicked;
        }

        button.spriteState = spriteState;
    }
}
