using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    //Load-New 결정하는 변수
    public static bool loaded;


    //세이브 변수들
    string json;
    private static readonly string SAVE_FOLDER = Application.dataPath + "/SaveAndLoad/";

    //포괄
    [SerializeField] GameObject player;

    //Health script 저장
    HealthObjects healthObject;
    private Health healthScript;
    [SerializeField] GameObject healthBar;
    private HealthBar healthBarScript;
                            



    void Awake()
    {
        //폴더가 존재하는지 확인하고 없으면 생성
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
        healthScript = player.GetComponent<Health>();
        healthBarScript = healthBar.GetComponent<HealthBar>();
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    //일단 f1 누르면 save, f2 누르면 load
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Load();
        }
    }
    private void Save()
    {
        SaveHealthScript();
    }

    private void Load()
    {
        LoadHealthScript();
    }
  
    //Health script 저장
    private class HealthObjects
    {
        public float maxHP;
        public float curHP;
    }
    public void SaveHealthScript()
    {
        healthObject = new HealthObjects
        {
            maxHP = healthScript.maxHP,
            curHP = healthScript.curHP
        };
        json = JsonUtility.ToJson(healthObject);
        File.WriteAllText(SAVE_FOLDER + "/HealthScriptSave.txt", json);
    }
    public void LoadHealthScript()
    {
        if (File.Exists(SAVE_FOLDER + "/HealthScriptSave.txt"))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + "/HealthScriptSave.txt");
            healthObject = JsonUtility.FromJson<HealthObjects>(saveString);

            healthScript.maxHP = healthObject.maxHP;
            healthScript.curHP = healthObject.curHP;
            //체력바 즉시 변경
            healthBarScript.SetMaxHealth(healthObject.maxHP);
            healthBarScript.SetHealth(healthObject.curHP);
        }
    }
}
