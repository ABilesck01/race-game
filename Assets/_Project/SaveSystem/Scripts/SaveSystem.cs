using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static PlayerData LoadPlayerData()
    {
        PlayerData data = new PlayerData();
        if(!PlayerPrefs.HasKey("player"))
            return data;

        string json = PlayerPrefs.GetString("player");
        Debug.Log(json);

        data = JsonUtility.FromJson<PlayerData>(json);
        return data;
    }

    public static void SavePlayerData(PlayerData playerData)
    {
        string json = JsonUtility.ToJson(playerData);
        Debug.Log(json);
        PlayerPrefs.SetString("player", json);
    }
}
