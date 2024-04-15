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

    public CharacterManager characterManager;
    public ShopManager shopManager;

    public int Money;
    public Text moneyText;

    public GameObject moneyAddedPopUp;
    public Text moneyAddedText;

    bool AntNestFound;
    bool ArmyTrenchFound;
    bool PiratesMet;
    bool CrusadeFound;
    bool CatacombFound;
    bool AtlantisFound;
    bool UndergroundTribeFound;
    bool LostWorldFound;
    bool EldoradoFound;
    bool TreasureFound;

    bool MadScientistLabFound;

    Item curItem;
    InventoryItem currentInventoryItem;


    //Tool UI Durability
    [SerializeField] Slider durabilitySlider;
    public Gradient gradient;
    public Image durabilityFill;
    [SerializeField] Image curToolImage;
    [SerializeField] GameObject NullMask;

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
        Money =1000000;
    }

    private void Update()
    {
        if(Player.Dead == true)
        {
            StartCoroutine(PlayerDead());
            moneyText.text = Money.ToString();
            //defaultShovelSpawned = false;

        }
        /*
        if (defaultShovelSpawned == false)
        {
            Debug.Log("Shovel Spawned");
            inventoryManager.SpawnNewItem(defaultShovel, firstSlotToolBelt);
            defaultShovelSpawned = true;
        }*/
        SetShop();
        moneyText.text = Money.ToString();
        curItem = Player.curItem;
        currentInventoryItem = toolManager.curToolInvenItem;
        ShowCurrentTool();
        //moneyText.text = Money.ToString();  
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

            
            //set Star;
        }
        else
        {
            curToolImage.sprite = null;

            durabilitySlider.maxValue = 100;

            durabilitySlider.value = 100;
            NullMask.SetActive(true);
        }
    }

    IEnumerator PlayerDead()
    {
        Halo.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        //reset inventory
        Halo.SetActive(false);
        StartCoroutine(PlayerReset());
    }
    IEnumerator PlayerReset()
    {
        //���� ������ ǥ�� ���� ȭ�� ǥ��
        BlackScreen.SetActive(true);
        bool inventoryReset = inventoryManager.EmptyAllObjects();
        //reset toolbelt
        bool toolBeltReset = toolManager.toolBeltReset();
        //resetlocation
        //reset character
        bool characterReset = characterManager.resetCharacter();
        if (inventoryReset == true && characterReset == true && toolBeltReset == true) //&& defaultShovelSpawned == false)
        {
            Player.Dead = false;
        }
        yield return new WaitForSeconds(3.0f);
        BlackScreen.SetActive(false);
        
    }

    public void MoneyAdded(int profit)
    {
        //�� ������ ���� �߰�
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

    }

}
