using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SaveLoadManager : MonoBehaviour
{
    //Load-New 결정하는 변수
    public static bool loaded;


    //세이브 변수들
    string json;
    private static readonly string SAVE_FOLDER = Application.dataPath + "/SaveAndLoad/";

    //포괄
    [SerializeField] GameObject player;

    //Health script 저장
    [SerializeField] GameObject healthBar;

    //GameManager
    [SerializeField] GameObject gameManager;

    //인벤 저장
    [SerializeField] GameObject inventoryManager;
    private InventoryManager IM;
    [SerializeField] GameObject toolManager;
    private ToolManager TM;

    public Item[] inventoryItemArray; //index -> item
    public Dictionary<Item,int> inventoryItemDictionary = new Dictionary<Item, int>();

    //맵 저장
    [SerializeField] TileBase railTile;
    [SerializeField] TileBase leftLadderTile;
    [SerializeField] TileBase rightLadderTile;
    [SerializeField] TileBase elevatorPassageTile;
    public Dictionary<GameObject,GameObject> topDic = new Dictionary<GameObject,GameObject>();
    public Dictionary<GameObject,GameObject> botDic = new Dictionary<GameObject,GameObject>();
    public GameObject elevatorTop;
    public GameObject elevatorBot;


    void Awake()
    {
        //폴더가 존재하는지 확인하고 없으면 생성
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
    //일단 f1 누르면 save, f22 누르면 load
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
            topKey = new List<Vector3>(),
            topValue = new List<Vector3>(),
            botKey = new List<Vector3>(),
            botValue = new List<Vector3>(),
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
        foreach(KeyValuePair<GameObject, GameObject> pair in topDic)
        {
            mapObject.topKey.Add(pair.Key.transform.position);
            if (pair.Value != null)
            {
                mapObject.topValue.Add(pair.Value.transform.position);
            }
            else
            {
                mapObject.topValue.Add(new Vector3(-1,1,0));
            }
        }
        foreach (KeyValuePair<GameObject, GameObject> pair in botDic)
        {
            mapObject.botKey.Add(pair.Key.transform.position);
            if (pair.Value != null)
            {
                mapObject.botValue.Add(pair.Value.transform.position);
            }
            else
            {
                mapObject.botValue.Add(new Vector3(-1, 1, 0));
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
        public List<Vector3> topKey;
        public List<Vector3> topValue;
        public List<Vector3> botKey;
        public List<Vector3> botValue;
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
            Dictionary<Vector3, Vector3> topLoadDic = mapObject.topKey.Zip(mapObject.topValue, (k, v) => new { k, v }).ToDictionary(a => a.k, a => a.v);
            foreach (KeyValuePair<Vector3, Vector3> pair in topLoadDic)
            {
                GameObject top = Instantiate(elevatorTop);
                top.transform.position = pair.Key;
                if(Vector3.Distance(new Vector3(-1,1,0), pair.Value) >= 0.1f) //짝이 있다
                {
                    GameObject bot = Instantiate(elevatorBot);
                    bot.transform.position = pair.Value;
                    top.GetComponent<Elevator>().isConnected = true;
                    top.GetComponent<Elevator>().pair = bot;
                    bot.GetComponent<Elevator>().isConnected = true;
                    bot.GetComponent<Elevator>().pair = top;
                    topDic.Add(top, bot);
                    botDic.Add(bot, top);
                }
                else
                {
                    topDic.Add(top, null);
                }
            }
            Dictionary<Vector3, Vector3> botLoadDic = mapObject.botKey.Zip(mapObject.botValue, (k, v) => new { k, v }).ToDictionary(a => a.k, a => a.v);
            foreach (KeyValuePair<Vector3, Vector3> pair in botLoadDic)
            {
                if (Vector3.Distance(new Vector3(-1, 1, 0), pair.Value) <= 0.1f) //짝이 없다
                {
                    GameObject bot = Instantiate(elevatorBot);
                    bot.transform.position = pair.Key;
                    botDic.Add(bot, null);
                }
            }
            //player.transform.position = mapObject.playerPos;


            //체력바 즉시 변경

        }
    }
}
