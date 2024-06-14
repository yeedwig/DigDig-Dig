using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    public Item[] inventoryItemArray; //index -> item
    public Dictionary<Item,int> inventoryItemDictionary = new Dictionary<Item, int>();

    //�� ����
    [SerializeField] TileBase railTile;
    [SerializeField] TileBase leftLadderTile;
    [SerializeField] TileBase rightLadderTile;
    [SerializeField] TileBase elevatorPassageTile;


    void Awake()
    {
        //������ �����ϴ��� Ȯ���ϰ� ������ ����
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
        IM = inventoryManager.GetComponent<InventoryManager>();
        TM = toolManager.GetComponent<ToolManager>();
        
        for(int i=0;i<inventoryItemArray.Length; i++)
        {
            inventoryItemDictionary.Add(inventoryItemArray[i], i);
        }
        
    }

    private void Start()
    {
        if (loaded)
        {
            MapLoadTest();
        }
    }

    // Update is called once per frame
    //�ϴ� f1 ������ save, f22 ������ load
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
        SaveInventory.saveInventory(inventoryManager.GetComponent<InventoryManager>(),toolManager.GetComponent<ToolManager>(),inventoryItemDictionary);
        MapObjects mapObject = new MapObjects
        {
            playerPos = player.transform.position,
            diggedKey = new List<Vector3Int>(),
            gangKey = new List<Vector3Int>(),
            railKey = new List<Vector3Int>(),
            ladderKey = new List<Vector3Int>(),
            ladderIsLeft = new List<bool>(),
            passageKey = new List<Vector3Int>(),
        };
        foreach(Vector3Int key in GangController.instance.gangDictionary.Keys)
        {
            mapObject.gangKey.Add(key);
        }
        foreach (KeyValuePair<Vector3Int,GameObject> pair in GroundDictionary.instance.groundDictionary)
        {
            Ground ground = pair.Value.GetComponent<Ground>();
            if(!ground.bc.enabled&&!ground.isBlank)
            {
                mapObject.diggedKey.Add(pair.Key);
            }
            if (TilemapManager.instance.railTilemap.GetTile(pair.Key) != null)
            {
                mapObject.railKey.Add(pair.Key);
            }
            if (TilemapManager.instance.elevatorPassageTilemap.GetTile(pair.Key) != null)
            {
                mapObject.passageKey.Add(pair.Key);
            }

            if (TilemapManager.instance.ladderTilemap.GetTile(pair.Key) == leftLadderTile)
            {
                mapObject.ladderKey.Add(pair.Key);
                mapObject.ladderIsLeft.Add(true);
            }
            else if (TilemapManager.instance.ladderTilemap.GetTile(pair.Key) == rightLadderTile)
            {
                mapObject.ladderKey.Add(pair.Key);
                mapObject.ladderIsLeft.Add(false);
            }
        }

        string json = JsonUtility.ToJson(mapObject);
        File.WriteAllText(SAVE_FOLDER + "/MapSave.txt", json);
    }

    public class MapObjects
    {
        public Vector3 playerPos;
        public List<Vector3Int> diggedKey;
        public List<Vector3Int> gangKey;
        public List<Vector3Int> railKey;
        public List<Vector3Int> ladderKey;
        public List<bool> ladderIsLeft;
        public List<Vector3Int> passageKey;
    }

    
    

    private void Load()
    {
        SaveHealth.loadHealth(player.GetComponent<Health>(), healthBar.GetComponent<HealthBar>());
        SaveGameManager.loadGameManager(gameManager.GetComponent<GameManager>());
        SaveInventory.loadInventory(inventoryManager.GetComponent<InventoryManager>(), toolManager.GetComponent<ToolManager>(),inventoryItemArray);
        
    }

    private void MapLoadTest()
    {
        if (File.Exists(SAVE_FOLDER + "/MapSave.txt"))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + "/MapSave.txt");
            MapObjects mapObject = JsonUtility.FromJson<MapObjects>(saveString);
            foreach(Vector3Int key in mapObject.diggedKey)
            {
                Ground ground = GroundDictionary.instance.groundDictionary[key].GetComponent<Ground>();
                ground.currentHealth = -100.0f;
                ground.bc.enabled = false;
                ground.ChangeSpriteByCurrentHealth();
            }
            foreach(Vector3Int key in  mapObject.gangKey)
            {
                GangController.instance.CreateGang(key);
                
            }
            foreach (Vector3Int key in mapObject.railKey)
            {
                TilemapManager.instance.railTilemap.SetTile(key, railTile);

            }
            foreach (Vector3Int key in mapObject.passageKey)
            {
                TilemapManager.instance.elevatorPassageTilemap.SetTile(key, elevatorPassageTile);

            }
            Dictionary<Vector3Int,bool> ladderDic = mapObject.ladderKey.Zip(mapObject.ladderIsLeft, (k, v) => new {k,v}).ToDictionary(a=>a.k,a=>a.v);
            foreach(KeyValuePair<Vector3Int,bool> pair in ladderDic)
            {
                if (pair.Value)
                {
                    TilemapManager.instance.ladderTilemap.SetTile(pair.Key,leftLadderTile);
                }
                else
                {
                    TilemapManager.instance.ladderTilemap.SetTile(pair.Key, rightLadderTile);
                }
            }
            player.transform.position = mapObject.playerPos;


            //ü�¹� ��� ����

        }
    }
}
