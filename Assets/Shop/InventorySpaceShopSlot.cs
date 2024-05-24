using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class InventorySpaceShopSlot : MonoBehaviour
{
    public Text nameTextSlot;
    public Text priceTextSlot;

    public int price;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private InventoryManager inventoryManager;

    [SerializeField] Text descriptionTxt;
    [SerializeField] Text statTxt;

    public AudioClip[] cashOutSound;
    public AudioClip[] buttonPressSound;
    void Start()
    {
        nameTextSlot.text = "Lv2. Bag";
        priceTextSlot.text = price.ToString();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void BuyItem()
    {
        if (gameManager.Money >= price)
        { 
            if(inventoryManager.currentInventoryLevel < 5)
            {
                gameManager.Money -= price;
                SoundFXManager.instance.PlaySoundFXClip(cashOutSound, transform, 1.5f);
                inventoryManager.AddInventorySlots();
                gameManager.updateMoney();
                if (inventoryManager.currentInventoryLevel == 1)
                {
                    price += 1000;
                    nameTextSlot.text = "Lv3. Bag";
                    priceTextSlot.text = price.ToString();
                }
                if (inventoryManager.currentInventoryLevel == 2)
                {
                    nameTextSlot.text = "Lv4. Bag";
                    price += 1000;
                    priceTextSlot.text = price.ToString();
                }
                if (inventoryManager.currentInventoryLevel == 3)
                {
                    nameTextSlot.text = "Lv5. Bag";
                    price += 1000;
                    priceTextSlot.text = price.ToString();
                }

                if (inventoryManager.currentInventoryLevel == 4)
                {
                    nameTextSlot.text = "Lv6. Bag";
                    price += 1000;
                    priceTextSlot.text = price.ToString();
                }

                if(inventoryManager.currentInventoryLevel == 5)
                {
                    nameTextSlot.text = "Inventory Mastered";
                    priceTextSlot.text = "Maxed Out";
                }

            }
            else if(inventoryManager.currentInventoryLevel >= 5)
            {
                return;
            }
           
            
            //돈빠져나가는 애니메이션
        }
        else
            return;
    }

    public void ShowDescription()
    {
        //Show Item Description
        SoundFXManager.instance.PlaySoundFXClip(buttonPressSound, transform, 1.5f);
        descriptionTxt.text = "Upgrades Inventory Size by 5";

        //statTxt.text = "Damage : " + item.damage.ToString() + "\n" + "Durability : " + item.durability.ToString() + "\n" + "Selling Price : " + item.price.ToString();
    }
}
