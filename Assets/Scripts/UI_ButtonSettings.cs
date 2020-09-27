using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ButtonSettings : MonoBehaviour
{
    public string player_prefs_key;

    public Sprite sprite_on;
    public Sprite sprite_off;
    public Image image;
    public Button button;
    public bool toggle = true;

    void Start()
    {
        image = gameObject.GetComponent<Image>();
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener( () => 
        { 
            press(); 
        });

        toggle = PlayerPrefs.GetInt(player_prefs_key) == 1;

        setSprite();
    }

    void press() {
        toggle = !toggle;
        PlayerPrefs.SetInt(player_prefs_key, boolToInt(toggle));
        
        setSprite();
    }

    private void setSprite() {
        if (toggle) { image.sprite = sprite_on; }
        else { image.sprite = sprite_off; }
    }

    private int boolToInt(bool input) {
        if (input) {return 1;}
        else {return 0; }
    }
}
