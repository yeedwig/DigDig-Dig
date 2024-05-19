using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    //Load-New �����ϴ� ����
    public static bool loaded;


    //���̺� ������
    string json;
    private static readonly string SAVE_FOLDER = Application.dataPath + "/SaveAndLoad/";

    //����
    [SerializeField] GameObject player;

    //Health script ����
    HealthObjects healthObject;
    private Health healthScript;
    [SerializeField] GameObject healthBar;
    private HealthBar healthBarScript;
                            



    void Awake()
    {
        //������ �����ϴ��� Ȯ���ϰ� ������ ����
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
    //�ϴ� f1 ������ save, f2 ������ load
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
  
    //Health script ����
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
            //ü�¹� ��� ����
            healthBarScript.SetMaxHealth(healthObject.maxHP);
            healthBarScript.SetHealth(healthObject.curHP);
        }
    }
}
