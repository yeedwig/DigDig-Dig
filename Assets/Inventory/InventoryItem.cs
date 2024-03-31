using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    
    public Image image;
    public Text countText;

    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;

    public float Durability;

    public void InitializeItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        if(item.isTool)
        {
            Durability = item.durability;
        }
        RefreshCount(); 
    }

    

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {

        transform.position = Input.mousePosition;
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }

    public void Damage(float damage)
    {
        Durability -= damage;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //여기 설렉트 하나 프레임 하나 넣기 그림
            GameObject imageDescription = GameObject.Find("ItemDescriptionInventoryImage");
            //imageDescription.SetActive(true);
            imageDescription.GetComponent<Image>().sprite = image.sprite;

            GameObject description = GameObject.Find("ItemInventoryDesriptionTxt");
            if(item.isTool)
            {
                description.GetComponent<Text>().text = item.name + "\n" + item.Description + "\n" + "Damage : " + item.damage + "\n" + "Durability : " + item.durability;
            }
            else
            {
                description.GetComponent<Text>().text = item.name + "\n" + item.Description + "\n" + "Price : " + item.price;
            }
            
        }
    }
}
