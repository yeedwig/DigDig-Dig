using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
        SaveHealth.saveHealth(player.GetComponent<Health>());
    }

    private void Load()
    {
        SaveHealth.loadHealth(player.GetComponent<Health>(), healthBar.GetComponent<HealthBar>());
    }
  
    //GameManager 스크립트 저장
    private class GameManagerObjects
    {
        public bool AntNestFound;
        public bool ArmyTrenchFound;
        public bool PiratesMet;
        public bool CrusadeFound;
        public bool CatacombFound;
        public bool AtlantisFound;
        public bool UndergroundTribeFound;
        public bool LostWorldFound;
        public bool EldoradoFound;
        public bool TreasureFound;
        public bool MadScientistLabFound;
        public int money;

        public int GangNum;
        public int LadderNum;
        public int RailNum;
        public int ElevatorDoorNum;
        public int ElevatorPassageNum;
    }

    public void SaveGameManagerScript()
    {

    }
}
