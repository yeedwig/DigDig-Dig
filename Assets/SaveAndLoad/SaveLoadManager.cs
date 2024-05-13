using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    //���̺� ������
    SaveObjects saveObject;
    string json;

    //���̺꿡 �ʿ��� ��ü��
    [SerializeField] GameObject saveloadtest;
    [SerializeField] GameObject player;



    void Awake()
    {
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
        saveObject = new SaveObjects
        {
            testInt = saveloadtest.GetComponent<SaveAndLoadTest>().testInt,
            playerPos = player.transform.position
        };
        json = JsonUtility.ToJson(saveObject);
        File.WriteAllText(Application.dataPath + "/save.txt", json);
        Debug.Log("Saved");
    }

    private void Load()
    {
        if(File.Exists(Application.dataPath + "/save.txt"))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/save.txt");
            saveObject = JsonUtility.FromJson<SaveObjects>(saveString);
        }
        saveloadtest.GetComponent<SaveAndLoadTest>().testInt=saveObject.testInt;
        player.transform.position = saveObject.playerPos;
        Debug.Log("Loaded");
    }
    //������ ���� �� ��ü�� ����
    private class SaveObjects 
    {
        public int testInt;
        public Vector3 playerPos;
    }
}
