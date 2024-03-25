using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemsToPickUp;

    public void PickupItem(int id)
    {
       bool result = inventoryManager.AddItem(itemsToPickUp[id]);

       if(result == true)
        {
            Debug.Log("ItemAdded");
        }
        else
        {
            Debug.Log("ITEM NOT ADDED");
        }
    }
}
