using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    //Load-New 결정하는 변수
    public static bool loaded;


    //세이브 변수들
    SaveObjects saveObject;
    string json;
    private static readonly string SAVE_FOLDER = Application.dataPath + "/SaveAndLoad/";

    //세이브에 필요한 객체들
    



    void Awake()
    {
        //폴더가 존재하는지 확인하고 없으면 생성
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
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
            
        };
        json = JsonUtility.ToJson(saveObject);
        File.WriteAllText(SAVE_FOLDER + "/save.txt", json);
    }

    private void Load()
    {
        if(File.Exists(SAVE_FOLDER + "/save.txt"))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + "/save.txt");
            saveObject = JsonUtility.FromJson<SaveObjects>(saveString);
        }
        
    }
    //저장할 변수 및 객체들 모음
    private class SaveObjects 
    {
        
    }
}
