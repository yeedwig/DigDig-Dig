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
    private static readonly string SAVE_FOLDER = Application.dataPath + "/SaveFiles/";

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

    //플레이어 위치 저장

    //해적 저장
    [SerializeField] GameObject pirateManager;


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
            SaveHealth.loadHealth(player.GetComponent<Health>(), healthBar.GetComponent<HealthBar>());
            SaveMap.loadMap(topDic, botDic, railTile, elevatorPassageTile, leftLadderTile, rightLadderTile, elevatorTop, elevatorBot);
            SaveGameManager.loadGameManager(gameManager.GetComponent<GameManager>());
            SaveInventory.loadInventory(inventoryManager.GetComponent<InventoryManager>(), toolManager.GetComponent<ToolManager>(), inventoryItemArray);
            SavePirate.load(pirateManager.GetComponent<PirateManager>());
            //player 위치는 map Generator에서 load 중
        }
    }

    // Update is called once per frame
    //일단 f1 누르면 save, f22 누르면 load
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Save();
        }*/
    }
    public void Save()
    {
        SaveHealth.saveHealth(player.GetComponent<Health>());
        SaveMap.saveMap(topDic, botDic, leftLadderTile, rightLadderTile);
        SaveGameManager.saveGameManager(gameManager.GetComponent<GameManager>());
        SaveInventory.saveInventory(inventoryManager.GetComponent<InventoryManager>(),toolManager.GetComponent<ToolManager>(),inventoryItemDictionary);
        SavePlayer.savePlayer(player);
        SavePirate.save(pirateManager.GetComponent<PirateManager>());
    }

}
