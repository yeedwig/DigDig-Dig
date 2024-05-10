using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerManager Player;
    public GameObject Halo;
    public GameObject BlackScreen;

    public InventoryManager inventoryManager;
    public ToolManager toolManager;
    public InventorySlot firstSlotToolBelt;
    public Item defaultShovel;
    public bool defaultShovelSpawned = true;

    public int chapter;

    public CharacterManager characterManager;
    public ShopManager shopManager;

    public int Money;
    public Text moneyText;

    public GameObject moneyAddedPopUp;
    public Text moneyAddedText;

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

    Item curItem;
    InventoryItem currentInventoryItem;
    public Text currentItemStat;


    //Tool UI Durability
    [SerializeField] Slider durabilitySlider;
    public Gradient gradient;
    public Image durabilityFill;
    [SerializeField] Image curToolImage;
    [SerializeField] GameObject NullMask;


    //구조물 관련
    public int GangNum;
    public int LadderNum;
    public int RailNum;
    public int ElevatorDoorNum;
    public int ElevatorPassageNum;

    public Text GangNumTxt;
    public Text LadderNumTxt;
    public Text RailNumTxt;
    public Text ElevatorDoorNumTxt;
    public Text ElevatorPassageNumTxt;
    private void Start()
    {
        AntNestFound = false;
        ArmyTrenchFound = false;
        PiratesMet = false;
        CrusadeFound = false;
        CatacombFound = false;
        AtlantisFound = false;
        UndergroundTribeFound = false;
        LostWorldFound = false;
        EldoradoFound = false;
        TreasureFound = false;
        MadScientistLabFound = false;
        Money =1000000;
    }

    private void Update()
    {
        if(Player.Dead == true)
        {
            StartCoroutine(PlayerDead());
            moneyText.text = Money.ToString();

        }
       
        SetShop();
        moneyText.text = Money.ToString();
        curItem = Player.curItem;
        currentInventoryItem = toolManager.curToolInvenItem;
        ShowCurrentTool();
        ShowStructureNum();
        //moneyText.text = Money.ToString();  
    }

    private void ShowStructureNum()
    {
        GangNumTxt.text = " X " + GangNum.ToString();
        LadderNumTxt.text = " X " + LadderNum.ToString();   
        RailNumTxt.text = " X " + RailNum.ToString();
        ElevatorDoorNumTxt.text = " X " + ElevatorDoorNum.ToString(); 
        ElevatorPassageNumTxt.text = " X " + ElevatorPassageNum.ToString();
    }

    private void resetStructureNum()
    {
        GangNum = 0;
        LadderNum = 0;
        RailNum = 0;    
        ElevatorPassageNum = 0;
        ElevatorDoorNum = 0;
    }
    private void ShowCurrentTool()
    {
        if (currentInventoryItem != null && curItem != null)
        {
            //setImage;
            curToolImage.sprite = currentInventoryItem.item.image;
            //setDurability
            NullMask.SetActive(false);
            //Debug.Log(curItem.durability + "/" + currentInventoryItem.Durability);
            
            durabilitySlider.maxValue = curItem.durability;
            durabilityFill.color = gradient.Evaluate(durabilitySlider.normalizedValue);
            durabilitySlider.value = currentInventoryItem.Durability;

            currentItemStat.text = "DMG : " + curItem.damage.ToString() + "\n";
            currentItemStat.text += "EFF : " + curItem.efficiency.ToString() + "\n";
            currentItemStat.text += curItem.Description;
            //set Star;
        }
        else
        {
            currentItemStat.text = "";
            curToolImage.sprite = null;

            durabilitySlider.maxValue = 100;

            durabilitySlider.value = 100;
            NullMask.SetActive(true);
        }
    }

    IEnumerator PlayerDead()
    {
        Halo.SetActive(true);
        Player.respawning = true;
        yield return new WaitForSeconds(2.0f);
        //reset inventory
        Halo.SetActive(false);
        StartCoroutine(PlayerReset());
    }
    IEnumerator PlayerReset()
    {
        //눈에 엑스자 표시 검은 화면 표시
        BlackScreen.SetActive(true);
        bool inventoryReset = inventoryManager.EmptyAllObjects();
        //reset toolbelt
        bool toolBeltReset = toolManager.toolBeltReset();
        //resetlocation

        //reset Health
        Player.curHP = Player.MaxHP;
        //reset character
        bool characterReset = characterManager.resetCharacter();
        if (inventoryReset == true && characterReset == true && toolBeltReset == true) //&& defaultShovelSpawned == false)
        {
            Player.Dead = false;
            defaultShovelSpawned = false;
        }
        yield return new WaitForSeconds(3.0f);
        BlackScreen.SetActive(false);
        Player.respawning = false;
        if (defaultShovelSpawned == false)
        {
            Debug.Log("Shovel Spawned");
            inventoryManager.SpawnNewItem(defaultShovel, firstSlotToolBelt);
            defaultShovelSpawned = true;
        }
    }


    public void MoneyAdded(int profit)
    {
        //돈 들어오는 사운드 추가
        StartCoroutine(ShowMoneyAdded(profit));
    }

    IEnumerator ShowMoneyAdded(int profit)
    {
        moneyAddedText.text = "+ " + profit.ToString();
        moneyAddedPopUp.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        moneyAddedPopUp.SetActive(false);
        moneyAddedText.text = "";
        Money += profit;
    }


    public void SetShop()
    {
        shopManager.AntNestFound = AntNestFound;
        shopManager.ArmyTrenchFound = ArmyTrenchFound;
        shopManager.PiratesMet = PiratesMet;
        shopManager.CrusadeFound = CrusadeFound;    
        shopManager.CatacombFound = CatacombFound;
        shopManager.AtlantisFound = AtlantisFound;
        shopManager.UndergroundTribeFound = UndergroundTribeFound;
        shopManager.LostWorldFound = LostWorldFound;
        shopManager.EldoradoFound = EldoradoFound;
        shopManager.TreasureFound = TreasureFound;
        shopManager.MadScientistLabFound = MadScientistLabFound;

    }

}
