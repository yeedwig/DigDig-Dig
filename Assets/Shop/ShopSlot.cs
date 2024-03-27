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

    [SerializeField] Text descriptionTxt;
    [SerializeField] Text statTxt;

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
            //돈빠져나가는 애니메이션
        }
        else
            return;
    }

    public void ShowDescription()
    {
        //Show Item Description
        descriptionTxt.text = item.name.ToString() + " : " + "\n" + item.Description.ToString();
        
        statTxt.text = "Damage : " + item.damage.ToString() + "\n" + "Durability : " + item.durability.ToString() + "\n" + "Selling Price : " + item.price.ToString();
    }
}
