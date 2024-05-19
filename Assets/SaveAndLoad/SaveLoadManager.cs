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
        SaveHealth.saveHealth(player.GetComponent<Health>());
    }

    private void Load()
    {
        SaveHealth.loadHealth(player.GetComponent<Health>(), healthBar.GetComponent<HealthBar>());
    }
  
    //GameManager ��ũ��Ʈ ����
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
