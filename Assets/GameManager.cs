using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerManager Player;
    public InventoryManager inventoryManager;
    public ToolManager toolManager;
    public CharacterManager characterManager;
    public ShopManager shopManager;

    public int Money;
    public Text moneyText;


    bool AntNestFound;
    bool ArmyTrenchFound;
    bool MadScientistLabFound;
    bool CrusadeFound;
    bool CatacombFound;
    bool AtlantisFound;
    bool UndergroundTribeFound;
    bool LostWorldFound;
    bool EldoradoFound;
    bool TreasureFound;

    
    private void Start()
    {
        Money = 0;
    }

    private void Update()
    {
        if(Player.Dead == true)
        {
            PlayerDead();
            moneyText.text = Money.ToString();
        }
        SetShop();
        //moneyText.text = Money.ToString();  
    }


    private void PlayerDead()
    {
        //reset inventory
        bool inventoryReset = inventoryManager.EmptyAllObjects();
        //reset toolbelt
        bool toolBeltReset = toolManager.toolBeltReset();
        //resetlocation
        //reset character
        bool characterReset = characterManager.resetCharacter();

        if (inventoryReset == true && characterReset == true && toolBeltReset == true)
        {
            Player.Dead = false;
        }
    }


    public void SetShop()
    {
        shopManager.AntNestFound = AntNestFound;
        shopManager.ArmyTrenchFound = ArmyTrenchFound;
        shopManager.MadScientistLabFound = MadScientistLabFound;
        shopManager.CrusadeFound = CrusadeFound;    
        shopManager.CatacombFound = CatacombFound;
        shopManager.AtlantisFound = AtlantisFound;
        shopManager.UndergroundTribeFound = UndergroundTribeFound;
        shopManager.LostWorldFound = LostWorldFound;
        shopManager.EldoradoFound = EldoradoFound;
        shopManager.TreasureFound = TreasureFound;

    }

}
