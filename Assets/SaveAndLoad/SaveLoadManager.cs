using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    //���̺� ������
    SaveObjects saveObject;
    string json;
    private static readonly string SAVE_FOLDER = Application.dataPath + "/SaveAndLoad/";

    //���̺꿡 �ʿ��� ��ü��
    [SerializeField] GameObject player;



    void Awake()
    {
        //������ �����ϴ��� Ȯ���ϰ� ������ ����
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
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
            playerPos = player.transform.position
        };
        json = JsonUtility.ToJson(saveObject);
        File.WriteAllText(SAVE_FOLDER + "/save.txt", json);
        Debug.Log("Saved");
    }

    private void Load()
    {
        if(File.Exists(SAVE_FOLDER + "/save.txt"))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + "/save.txt");
            saveObject = JsonUtility.FromJson<SaveObjects>(saveString);
        }
        player.transform.position = saveObject.playerPos;
    }
    //������ ���� �� ��ü�� ����
    private class SaveObjects 
    {
        public int testInt;
        public Vector3 playerPos;
    }
}
