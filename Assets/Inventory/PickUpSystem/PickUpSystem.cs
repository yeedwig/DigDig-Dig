using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField]
    private InventoryManager inventoryManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickableItem PickUpItem = collision.GetComponent<PickableItem>();
        if (PickUpItem != null)
        {
            inventoryManager.AddItem(PickUpItem.item);
            PickUpItem.DestroyItem();
        }
    }
}
