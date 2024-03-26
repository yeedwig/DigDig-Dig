using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    public Text priceTextSlot;
    
    public int price;
    [SerializeField] Item item;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private InventoryManager inventoryManager;
    

    // Start is called before the first frame update
    void Start()
    {
        priceTextSlot.text = price.ToString();
    }

    public void BuyItem()
    {
        if (gameManager.Money >= price)
        {
            if (inventoryManager.AddItem(item))
                gameManager.Money -= price;
            else
                return;
            //������������ �ִϸ��̼�
        }
        else
            return;
    }

    public void ShowDescription()
    {
        //Show Item Description
    }
}
