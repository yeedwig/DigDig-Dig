using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SaveInventory 
{
    private static readonly string SAVE_FOLDER = Application.dataPath + "/SaveAndLoad/";
    public class InventoryClass
    {
        //인벤
        public Item[] item;
        public int[] itemCount;
        public float[] durability;
        public int currentInventoryLevel;

        //툴 벨트
        public Item[] beltItem;
        public int[] beltItemCount;
        public float[] beltDurability;
    }

    public static void saveInventory(InventoryManager IM,ToolManager TM)
    {
        int saveIndex = 0;
        InventoryClass inventorySaveObject = new InventoryClass
        {
            item = new Item[0],
            itemCount = new int[0],
            durability = new float[0],
            currentInventoryLevel = IM.currentInventoryLevel,
            beltItem = new Item[0],
            beltItemCount = new int[0],
            beltDurability = new float[0]
        };
        for (int i = 0; i < IM.inventorySlotsLength; i++)
        {
            InventorySlot slot = IM.inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null)
            {
                Array.Resize(ref inventorySaveObject.item, saveIndex + 1);
                Array.Resize(ref inventorySaveObject.itemCount, saveIndex + 1);
                Array.Resize(ref inventorySaveObject.durability, saveIndex + 1);
                if (itemInSlot.item.isTool)
                {
                    inventorySaveObject.durability[saveIndex] = itemInSlot.Durability;
                }
                inventorySaveObject.item[saveIndex] = itemInSlot.item;
                inventorySaveObject.itemCount[saveIndex++] = itemInSlot.count;
            }
        }
        saveIndex = 0;
        for (int i = 0; i < 6; i++)
        {
            InventorySlot slot = TM.ToolBeltInventory[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                Array.Resize(ref inventorySaveObject.beltItem, saveIndex + 1);
                Array.Resize(ref inventorySaveObject.beltDurability, saveIndex + 1);
                Array.Resize(ref inventorySaveObject.beltItemCount, saveIndex + 1);
                inventorySaveObject.beltItem[saveIndex] = itemInSlot.item;
                inventorySaveObject.beltDurability[saveIndex] = itemInSlot.Durability;
                inventorySaveObject.beltItemCount[saveIndex++] = itemInSlot.count;
            }
        }
        string json = JsonUtility.ToJson(inventorySaveObject);
        File.WriteAllText(SAVE_FOLDER + "/InventorySave.txt", json);
    }

    public static void loadInventory(InventoryManager IM, ToolManager TM)
    {
        if (File.Exists(SAVE_FOLDER + "/InventorySave.txt"))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + "/InventorySave.txt");
            InventoryClass load = JsonUtility.FromJson<InventoryClass>(saveString);
            IM.currentInventoryLevel = load.currentInventoryLevel;
            IM.AddInventorySlots();
            for (int i = 0; i < load.item.Length; i++)
            {
                InventorySlot slot = IM.inventorySlots[i];
                IM.SpawnNewItem(load.item[i], slot);
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (load.item[i].stackable)
                {
                    itemInSlot.count = load.itemCount[i];
                    itemInSlot.RefreshCount();
                }
                if (load.item[i].isTool)
                {
                    itemInSlot.Durability = load.durability[i];
                }
            }

            TM.toolBeltReset();
            for (int i = 0; i < load.beltItem.Length; i++)
            {
                InventorySlot slot = TM.ToolBeltInventory[i];
                IM.SpawnNewItem(load.beltItem[i], slot);
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (load.beltItem[i].stackable)
                {
                    itemInSlot.count = load.beltItemCount[i];
                    itemInSlot.RefreshCount();
                }
                if (load.beltItem[i].isTool)
                {
                    itemInSlot.Durability = load.beltDurability[i];
                }
            }
        }
    }
}
