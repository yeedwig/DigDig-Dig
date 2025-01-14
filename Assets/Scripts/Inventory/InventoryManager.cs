using System.Collections;
using System.Collections.Generic;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public int currentInventoryLevel;

    public int inventorySlotsLength;

    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public PlayerManager Player;
    public GameManager GM;

    public GameObject InventoryContentParent;
    public GameObject emptyInventorySlot;

    public Image inventoryDescriptionImage;
    //public int maxStack = 5;

    public GameObject BagFullMessage;
    public Text BagFullMessageText;

    public AudioClip[] cantBuySound;
    public AudioClip[] coinAddedSound;
    public float fxvolume;

    public int maxStructure = 100;
    public void Start()
    {
        inventorySlotsLength = 5;
        currentInventoryLevel = 0;
        
        /*
        for(int i=0; i<inventorySlotsLength; i++)
        {
            inventorySlots[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < 5; i++)
        {
            inventorySlots[i].gameObject.SetActive(true);
        }*/
        //var tempColor = inventoryDescriptionImage.color;
        //tempColor.a = 0;
        //inventoryDescriptionImage.color = tempColor;
    }
    public bool AddItem(Item item)
    {
        if(item.isStructure)
        {
            if(item.Name =="Mine")
            {
                if(GM.GangNum <maxStructure)
                {
                    GM.GangNum++;
                    return true;
                }
            }
            if (item.Name == "Ladder")
            {
                if (GM.LadderNum < maxStructure)
                {
                    GM.LadderNum++;
                    return true;
                }
            }
            if (item.Name == "Rail")
            {
                if (GM.RailNum < maxStructure)
                {
                    GM.RailNum++;
                    return true;
                }
            }
            if (item.Name == "ElevatorDoor")
            {
                if (GM.ElevatorDoorNum < maxStructure)
                {
                    GM.ElevatorDoorNum++;
                    return true;
                }
            }
            if (item.Name == "ElevatorPassage")
            {
                if (GM.ElevatorPassageNum < maxStructure)
                {
                    GM.ElevatorPassageNum++;
                    return true;
                }
            }
        }
        else
        {
            for (int i = 0; i < inventorySlotsLength; i++)
            {
                InventorySlot slot = inventorySlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                //여기 maxStack 바꾸게
                if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < item.maxStack && itemInSlot.item.stackable == true)
                {
                    itemInSlot.count++;
                    itemInSlot.RefreshCount();
                    return true;
                }
            }

            //Find Empty SLot
            for (int i = 0; i < inventorySlotsLength; i++)
            {
                InventorySlot slot = inventorySlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot == null)
                {
                    SpawnNewItem(item, slot);
                    return true;
                }
            }
        }
        
        //SoundFXManager.instance.PlaySoundFXClip(cantBuySound, transform, fxvolume);
        StartCoroutine(MessageTimer());
        return false;
    }

    public void AddInventorySlots()
    {
        if(currentInventoryLevel == 0)
        {
            for(int i=5; i<10; i++)
            {
                inventorySlots[i].gameObject.SetActive(true);
            }
            inventorySlotsLength = 10;
            currentInventoryLevel++;
        }

        else if(currentInventoryLevel == 1)
        {
            for (int i = 10; i < 15; i++)
            {
                inventorySlots[i].gameObject.SetActive(true);
            }
            inventorySlotsLength = 15;
            currentInventoryLevel++;
        }

        else if(currentInventoryLevel == 2)
        {
            for (int i = 15; i < 20; i++)
            {
                inventorySlots[i].gameObject.SetActive(true);
            }
            inventorySlotsLength = 20;
            currentInventoryLevel++;
        }

        else if (currentInventoryLevel == 3)
        {
            for (int i = 20; i < 25; i++)
            {
                inventorySlots[i].gameObject.SetActive(true);
            }
            inventorySlotsLength = 25;
            currentInventoryLevel++;
        }

        else if (currentInventoryLevel == 4)
        {
            for (int i = 25; i < 30; i++)
            {
                inventorySlots[i].gameObject.SetActive(true);
            }
            inventorySlotsLength = 30;
            currentInventoryLevel++;
        }

        else if (currentInventoryLevel == 5)
        {
            currentInventoryLevel++;
        }

    }

    public void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }

    public bool EmptyAllObjects()
    {
        int totalPrice = 0;
        for(int i = 0; i < inventorySlotsLength; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot != null)
            {
                if (itemInSlot.item.stackable)
                {
                    //totalPrice += itemInSlot.item.price * itemInSlot.count;
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    //totalPrice += itemInSlot.item.price;
                    Destroy(itemInSlot.gameObject);
                }
            }

        }
        //여기서 비율 조정, 죽어서 태어나는거면 뭐 /5 그냥 파는거면 /1

        GM.MoneyAdded(totalPrice);
        return true;
    }

    IEnumerator MessageTimer()
    {
        BagFullMessage.SetActive(true);
        BagFullMessageText.text = "Inventory Full!";
        yield return new WaitForSeconds(1.0f);
        BagFullMessage.SetActive(false);
    }

    public void SellAllObjects()
    {
        int totalPrice = 0;
        for (int i = 0; i < inventorySlotsLength; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                if (itemInSlot.item.stackable)
                {
                    totalPrice += itemInSlot.item.price * itemInSlot.count;
                    //GM.MoneyAdded(itemInSlot.item.price * itemInSlot.count);

                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    totalPrice += itemInSlot.item.price;
                    //GM.MoneyAdded(itemInSlot.item.price);
                    Destroy(itemInSlot.gameObject);
                }
            }

        }
        //여기서 비율 조정, 죽어서 태어나는거면 뭐 /5 그냥 파는거면 /1
        SoundFXManager.instance.PlaySoundFXClip(coinAddedSound, transform, 1.0f);
        GM.MoneyAdded(totalPrice);
        GM.updateMoney();
    }

    public bool SearchInventory(Item item)
    {
        for(int i=0; i<inventorySlotsLength; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot.item.name == item.name)
            {
                if(itemInSlot.item.isKey)//use item if is key
                {
                    if(itemInSlot.item.Name == "MoleCard")
                    {

                    }
                    else
                    {
                        Destroy(itemInSlot);
                    }
                    
                }
                return true;
            }
        }

        return false;
    }
}
