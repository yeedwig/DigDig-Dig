using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveGameManager
{
    private static readonly string SAVE_FOLDER = Application.dataPath + "/SaveAndLoad/";

    //curtool 관련 추가해야 될지도
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

    public static void saveGameManager(GameManager gameManager)
    {
        GameManagerObjects gameManagerObject = new GameManagerObjects
        {
            AntNestFound = gameManager.AntNestFound,
            ArmyTrenchFound = gameManager.ArmyTrenchFound,
            PiratesMet = gameManager.PiratesMet,
            CrusadeFound = gameManager.CrusadeFound,
            CatacombFound = gameManager.CatacombFound,
            AtlantisFound = gameManager.AtlantisFound,
            UndergroundTribeFound = gameManager.UndergroundTribeFound,
            LostWorldFound = gameManager.LostWorldFound,
            EldoradoFound = gameManager.EldoradoFound,
            TreasureFound = gameManager.TreasureFound,
            MadScientistLabFound = gameManager.MadScientistLabFound,
            money = gameManager.Money,
            GangNum = gameManager.GangNum,
            LadderNum = gameManager.LadderNum,
            RailNum = gameManager.RailNum,
            ElevatorDoorNum = gameManager.ElevatorDoorNum,
            ElevatorPassageNum = gameManager.ElevatorPassageNum
        };
        string json = JsonUtility.ToJson(gameManagerObject);
        File.WriteAllText(SAVE_FOLDER + "/GameManagerSave.txt", json);
    }

    public static void loadGameManager(GameManager gameManager)
    {
        if (File.Exists(SAVE_FOLDER + "/GameManagerSave.txt"))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + "/GameManagerSave.txt");
            GameManagerObjects gameManagerObject = JsonUtility.FromJson<GameManagerObjects>(saveString);

            gameManager.AntNestFound = gameManagerObject.AntNestFound;
            gameManager.ArmyTrenchFound = gameManagerObject.ArmyTrenchFound;
            gameManager.PiratesMet = gameManagerObject.PiratesMet;
            gameManager.CrusadeFound = gameManagerObject.CrusadeFound;
            gameManager.CatacombFound = gameManagerObject.CatacombFound;
            gameManager.AtlantisFound = gameManagerObject.AtlantisFound;
            gameManager.UndergroundTribeFound = gameManagerObject.UndergroundTribeFound;
            gameManager.LostWorldFound =  gameManagerObject.LostWorldFound;
            gameManager.EldoradoFound = gameManagerObject.EldoradoFound;
            gameManager.TreasureFound = gameManagerObject.TreasureFound;
            gameManager.MadScientistLabFound = gameManagerObject.MadScientistLabFound;
            gameManager.Money = gameManagerObject.money;
            gameManager.GangNum = gameManagerObject.GangNum;
            gameManager.LadderNum = gameManagerObject.LadderNum;
            gameManager.RailNum = gameManagerObject.RailNum;
            gameManager.ElevatorDoorNum = gameManagerObject.ElevatorDoorNum;
            gameManager.ElevatorPassageNum = gameManagerObject.ElevatorPassageNum;
        }
    }
}
