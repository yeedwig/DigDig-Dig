using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public bool isToolSlot;

    public Image image;
    public Color selectedColor, defaultColor;

    private void Awake()
    {
        Deselect();
    }

    public void Select()
    {
        image.color = selectedColor;
    }

    public void Deselect()
    {
        image.color = defaultColor;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(isToolSlot)
        {
            if (eventData.pointerDrag.GetComponent<InventoryItem>().item.isTool == true && transform.childCount == 0)
            {
                InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
                inventoryItem.parentAfterDrag = transform;
            }
        }
        else
        {
            if(transform.childCount == 0)
            {   
                InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
                inventoryItem.parentAfterDrag = transform;
            }
        }
        
    }
}
