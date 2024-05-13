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
    //�ϴ� o ������ save
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
    //������ ���� �� ��ü�� ����
    private class SaveObjects 
    {
        public int testInt;
        public Vector3 playerPos;
    }
}
