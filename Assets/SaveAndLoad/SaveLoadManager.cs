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

        //�κ��丮 ���� �׽�Ʈ ����
        if (Input.GetKeyDown(KeyCode.F3))
        {
            int saveIndex=0;
            InventoryClass inventorySaveObject = new InventoryClass
            {
                item = new Item[0],
                itemCount = new int[0],
                durability = new float[0],
                currentInventoryLevel = IM.currentInventoryLevel,
                beltItem = new Item[0],
                beltItemCount = new int[0],
                beltDurability = new float[0]
            };
            for (int i = 0; i < IM.inventorySlotsLength; i++)
            {
                InventorySlot slot = IM.inventorySlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

                if(itemInSlot != null)
                {
                    Array.Resize(ref inventorySaveObject.item, saveIndex + 1);
                    Array.Resize(ref inventorySaveObject.itemCount, saveIndex + 1);
                    Array.Resize(ref inventorySaveObject.durability, saveIndex + 1);
                    if (itemInSlot.item.isTool)
                    {
                        inventorySaveObject.durability[saveIndex] = itemInSlot.Durability;
                    }
                    inventorySaveObject.item[saveIndex] = itemInSlot.item;
                    inventorySaveObject.itemCount[saveIndex++] = itemInSlot.count;
                }
            }
            saveIndex = 0;
            for(int i=0;i<6; i++)
            {
                InventorySlot slot = TM.ToolBeltInventory[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if(itemInSlot != null)
                {
                    Array.Resize(ref inventorySaveObject.beltItem, saveIndex + 1);
                    Array.Resize(ref inventorySaveObject.beltDurability, saveIndex + 1);
                    Array.Resize(ref inventorySaveObject.beltItemCount, saveIndex + 1);
                    inventorySaveObject.beltItem[saveIndex] = itemInSlot.item;
                    inventorySaveObject.beltDurability[saveIndex] = itemInSlot.Durability;
                    inventorySaveObject.beltItemCount[saveIndex++] = itemInSlot.count;
                }
            }
            string json = JsonUtility.ToJson(inventorySaveObject);
            File.WriteAllText(SAVE_FOLDER + "/InventorySave.txt", json);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            if (File.Exists(SAVE_FOLDER + "/InventorySave.txt"))
            {
                string saveString = File.ReadAllText(SAVE_FOLDER + "/InventorySave.txt");
                InventoryClass load = JsonUtility.FromJson<InventoryClass>(saveString);
                IM.currentInventoryLevel = load.currentInventoryLevel;
                IM.AddInventorySlots();
                for(int i = 0; i < load.item.Length; i++)
                {
                    InventorySlot slot = IM.inventorySlots[i];
                    IM.SpawnNewItem(load.item[i], slot);
                    InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                    if (load.item[i].stackable)
                    {
                        itemInSlot.count = load.itemCount[i];
                        itemInSlot.RefreshCount();
                    }
                    if (load.item[i].isTool)
                    {
                        itemInSlot.Durability = load.durability[i];
                    }
                }
                

                for (int i =0; i< load.beltItem.Length; i++)
                {
                    InventorySlot slot = TM.ToolBeltInventory[i];
                    InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                    if (itemInSlot != null) { 
                        Destroy(itemInSlot.gameObject);
                    }
                    IM.SpawnNewItem(load.beltItem[i], slot);
                    itemInSlot = slot.GetComponentInChildren<InventoryItem>();

                    if (load.beltItem[i].stackable)
                    {
                        itemInSlot.count = load.beltItemCount[i];
                        itemInSlot.RefreshCount();
                    }
                    if (load.beltItem[i].isTool)
                    {
                        itemInSlot.Durability = load.beltDurability[i];
                    }
                }
            }
        }
    }
    private void Save()
    {
        SaveHealth.saveHealth(player.GetComponent<Health>());
        SaveGameManager.saveGameManager(gameManager.GetComponent<GameManager>());
    }

    private void Load()
    {
        SaveHealth.loadHealth(player.GetComponent<Health>(), healthBar.GetComponent<HealthBar>());
        SaveGameManager.loadGameManager(gameManager.GetComponent<GameManager>());
    }

    public class InventoryClass
    {
        //�κ�
        public Item[] item;
        public int[] itemCount;
        public float[] durability;
        public int currentInventoryLevel;

        //�� ��Ʈ
        public Item[] beltItem;
        public int[] beltItemCount;
        public float[] beltDurability;
    }
}
