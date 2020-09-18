using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public int biden_level_1 =      0;
    public int biden_level_2 =      0;
    public int biden_level_3 =      0;
    public int biden_level_4 =      0;
    public int biden_level_5 =      0;
    public int biden_level_6 =      0;
    public int biden_player_count = 0;
    public int trump_level_1 =      0;
    public int trump_level_2 =      0;
    public int trump_level_3 =      0;
    public int trump_level_4 =      0;
    public int trump_level_5 =      0;
    public int trump_level_6 =      0;
    public int trump_player_count = 0;

    public bool loaded;
    public bool success;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Main");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }


}
