using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveHealth
{
    private static readonly string SAVE_FOLDER = Application.dataPath + "/SaveFiles/";
    private class HealthObjects
    {
        public float maxHP;
        public float curHP;
    }

    public static void saveHealth(Health health)
    {
        HealthObjects healthObject = new HealthObjects
        {
            maxHP = health.maxHP,
            curHP = health.curHP
        };
        string json = JsonUtility.ToJson(healthObject);
        File.WriteAllText(SAVE_FOLDER + "/HealthSave.txt", json);
    }

    public static void loadHealth(Health health, HealthBar healthBar)
    {
        if (File.Exists(SAVE_FOLDER + "/HealthSave.txt"))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + "/HealthSave.txt");
            HealthObjects healthObject = JsonUtility.FromJson<HealthObjects>(saveString);

            health.maxHP = healthObject.maxHP;
            health.curHP = healthObject.curHP;
            //체력바 즉시 변경
            healthBar.SetMaxHealth(healthObject.maxHP);
            healthBar.SetHealth(healthObject.curHP);
        }
    }
}
