using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrashSlot : MonoBehaviour, IDropHandler
{
    public AudioClip[] TrashSound;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<InventoryItem>().item != null && eventData.pointerDrag.GetComponent<InventoryItem>().item.isKey == false)
        {
            SoundFXManager.instance.PlaySoundFXClip(TrashSound, transform, 1.5f);
            Debug.Log("Trash");
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();

            Destroy(inventoryItem.gameObject);

        }
        else
            Debug.Log("Not in Loop!");

    }
}
