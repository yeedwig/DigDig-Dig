using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashSlot : MonoBehaviour
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<InventoryItem>().item != null)
        {
            Debug.Log("Trash");
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();

            inventoryItem.parentAfterDrag = transform;

            Destroy(this.GetComponentInChildren<InventoryItem>());
        }
        else
            Debug.Log("Not in Loop!");

    }
}
