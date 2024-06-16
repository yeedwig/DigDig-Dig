using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SavePirate
{
    private static readonly string SAVE_FOLDER = Application.dataPath + "/SaveAndLoad/";

    private class saveClass
    {
        public bool shovelGiven;
        public bool[] mapPiecesFound;
    }

    public static void save(PirateManager pm)
    {
        saveClass temp = new saveClass
        {
            shovelGiven = pm.shovelGiven,
            mapPiecesFound = pm.mapPiecesFound
        };
        string json = JsonUtility.ToJson(temp);
        File.WriteAllText(SAVE_FOLDER + "/PirateSave.txt", json);
    }
    public static void load(PirateManager pm)
    {
        if (File.Exists(SAVE_FOLDER + "/PirateSave.txt"))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + "/PirateSave.txt");
            saveClass temp = JsonUtility.FromJson<saveClass>(saveString);

            pm.shovelGiven = temp.shovelGiven;
            pm.mapPiecesFound = temp.mapPiecesFound;
        }
    }
}
