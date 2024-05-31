using System;
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
    [SerializeField] GameObject healthBar;

    //GameManager
    [SerializeField] GameObject gameManager;

    //�κ� ����
    [SerializeField] GameObject inventoryManager;
    private InventoryManager IM;
    [SerializeField] GameObject toolManager;
    private ToolManager TM;
                            



    void Awake()
    {
        //������ �����ϴ��� Ȯ���ϰ� ������ ����
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
        IM = inventoryManager.GetComponent<InventoryManager>();
        TM = toolManager.GetComponent<ToolManager>();
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
        SaveHealth.saveHealth(player.GetComponent<Health>());
        SaveGameManager.saveGameManager(gameManager.GetComponent<GameManager>());
        SaveInventory.saveInventory(inventoryManager.GetComponent<InventoryManager>(),toolManager.GetComponent<ToolManager>());
    }

    private void Load()
    {
        SaveHealth.loadHealth(player.GetComponent<Health>(), healthBar.GetComponent<HealthBar>());
        SaveGameManager.loadGameManager(gameManager.GetComponent<GameManager>());
        SaveInventory.loadInventory(inventoryManager.GetComponent<InventoryManager>(), toolManager.GetComponent<ToolManager>());
    }
}
