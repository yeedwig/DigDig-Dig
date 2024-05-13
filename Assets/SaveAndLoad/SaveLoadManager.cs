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
    //일단 o 누르면 save
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Save();
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
        File.WriteAllText(Application.dataPath + "SaveAndLoad/save.txt", json);
        Debug.Log("Saved");
    }

    private void Load()
    {

    }
    //저장할 변수 및 객체들 모음
    private class SaveObjects 
    {
        public int testInt;
        public Vector3 playerPos;
    }
}
