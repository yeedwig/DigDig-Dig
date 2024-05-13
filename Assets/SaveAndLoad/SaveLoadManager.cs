using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    //세이브 변수들
    SaveObjects saveObject;
    string json;

    //세이브에 필요한 객체들
    [SerializeField] GameObject saveloadtest;
    [SerializeField] GameObject player;



    void Awake()
    {
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
    //저장할 변수 및 객체들 모음
    private class SaveObjects 
    {
        public int testInt;
        public Vector3 playerPos;
    }
}
