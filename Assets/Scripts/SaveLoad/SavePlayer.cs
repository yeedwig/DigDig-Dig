using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SavePlayer 
{
    private static readonly string SAVE_FOLDER = Application.dataPath + "/SaveFiles/";
    
    public class PlayerObjects
    {
        public Vector3 playerPos;
    }

    public static void savePlayer(GameObject player)
    {
        PlayerObjects playerPos = new PlayerObjects
        {
            playerPos = player.transform.position,
        };
        string json = JsonUtility.ToJson(playerPos);
        File.WriteAllText(SAVE_FOLDER + "/PlayerSave.txt", json);
    }

    public static Vector3 loadPlayer(GameObject r)
    {
        if (File.Exists(SAVE_FOLDER + "/PlayerSave.txt"))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + "/PlayerSave.txt");
            PlayerObjects playerObject = JsonUtility.FromJson<PlayerObjects>(saveString);
            return playerObject.playerPos;
        }
        else
        {
            return r.transform.position;
        }
    } 
}
