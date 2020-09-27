using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;

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

    public DatabaseReference reference;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Main");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void AddToValue(string key)
    {
        DatabaseReference data_reference = reference.Child(key);

        data_reference.RunTransaction(data =>
        {
            if (data.Value == null) {
              data.Value = PlayerPrefs.GetInt(key) + 1;
            }
            else {
              data.Value = int.Parse(data.Value.ToString()) + 1;
            }

            return TransactionResult.Success(data);
        });
    }
}
