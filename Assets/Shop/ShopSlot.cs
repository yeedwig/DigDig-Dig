using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    public Text nameTextSlot;
    public Text priceTextSlot;
    
    public int price;
    [SerializeField] Item item;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private InventoryManager inventoryManager;

    [SerializeField] Text descriptionTxt;
    [SerializeField] Text statTxt;

    public AudioClip[] cashOutSound;
    public AudioClip[] buttonPressSound;
    // Start is called before the first frame update
    void Start()
    {
        nameTextSlot.text = item.name;
        price = item.price; 
        priceTextSlot.text = price.ToString();
    }

    public void BuyItem()
    {
        if (gameManager.Money >= price)
        {
            if (inventoryManager.AddItem(item))
            {
                SoundFXManager.instance.PlaySoundFXClip(cashOutSound, transform, 1.5f);
                gameManager.Money -= price;
            }
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
        SoundFXManager.instance.PlaySoundFXClip(buttonPressSound, transform, 1.5f);
        descriptionTxt.text = item.name.ToString() + " : " + "\n" + item.Description.ToString();
        
        statTxt.text = "Damage : " + item.damage.ToString() + "\n" + "Durability : " + item.durability.ToString() + "\n" + "Selling Price : " + item.price.ToString();
    }
}
