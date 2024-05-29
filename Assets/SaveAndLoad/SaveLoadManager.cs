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
                            



    void Awake()
    {
        //������ �����ϴ��� Ȯ���ϰ� ������ ����
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
        IM = inventoryManager.GetComponent<InventoryManager>();
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
            InventoryClass inventorySaveObject = new InventoryClass
            {
                item = new Item[IM.inventorySlotsLength],
                itemRange = 0,
                itemCount = new int[IM.inventorySlotsLength]
            };
            for (int i = 0; i < IM.inventorySlotsLength; i++)
            {
                InventorySlot slot = IM.inventorySlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

                if(itemInSlot != null)
                {
                    inventorySaveObject.item[inventorySaveObject.itemRange] = itemInSlot.item;
                    inventorySaveObject.itemCount[inventorySaveObject.itemRange++] = itemInSlot.count;
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
                for(int i = 0; i < load.itemRange; i++)
                {
                    Debug.Log(load.item[i].name);
                    Debug.Log(load.itemCount[i]);
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
        public Item[] item;
        public int itemRange = 0;
        public int[] itemCount;
    }
}
