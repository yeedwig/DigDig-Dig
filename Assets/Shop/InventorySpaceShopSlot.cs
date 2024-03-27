using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class InventorySpaceShopSlot : MonoBehaviour
{
    public Text priceTextSlot;

    public int price;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private InventoryManager inventoryManager;

    [SerializeField] Text descriptionTxt;
    [SerializeField] Text statTxt;

    void Start()
    {
        priceTextSlot.text = price.ToString();
    }

    public void BuyItem()
    {
        if (gameManager.Money >= price)
        { 
            if(inventoryManager.currentInventoryLevel < 5)
            {
                gameManager.Money -= price;
                inventoryManager.AddInventorySlots();
                if (inventoryManager.currentInventoryLevel == 1)
                {
                    price += 1000;
                    priceTextSlot.text = price.ToString();
                }
                if (inventoryManager.currentInventoryLevel == 2)
                {
                    price += 1000;
                    priceTextSlot.text = price.ToString();
                }
                if (inventoryManager.currentInventoryLevel == 3)
                {
                    price += 1000;
                    priceTextSlot.text = price.ToString();
                }

                if (inventoryManager.currentInventoryLevel == 4)
                {
                    price += 1000;
                    priceTextSlot.text = price.ToString();
                }

                if(inventoryManager.currentInventoryLevel == 5)
                {
                    priceTextSlot.text = "Maxed Out";
                }

            }
            else if(inventoryManager.currentInventoryLevel >= 5)
            {
                return;
            }
           
            
            //������������ �ִϸ��̼�
        }
        else
            return;
    }

    public void ShowDescription()
    {
        //Show Item Description
        descriptionTxt.text = "Upgrades Inventory Size by 5";

        //statTxt.text = "Damage : " + item.damage.ToString() + "\n" + "Durability : " + item.durability.ToString() + "\n" + "Selling Price : " + item.price.ToString();
    }
}
