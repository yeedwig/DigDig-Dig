using System.Collections;
using System.Collections.Generic;
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

    public int maxStack = 5;

    public void Start()
    {
        inventorySlotsLength = 5;
        currentInventoryLevel = 0;
    }
    public bool AddItem(Item item)
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
        for (int i=0; i<inventorySlotsLength; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }

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
        for(int i = 0; i < inventorySlotsLength; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot != null)
            {
                if (itemInSlot.item.stackable)
                {
                    GM.Money += itemInSlot.item.price * itemInSlot.count;

                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    GM.Money += itemInSlot.item.price;
                    Destroy(itemInSlot.gameObject);
                }
            }

        }
        return true;
    }

    public void SellAllObjects()
    {

    }
}
