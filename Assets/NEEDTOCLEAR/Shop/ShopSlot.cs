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
        //gameManager = FindFirstObjectByType<GameManager>();
    }

    public void BuyItem()
    {
        if (gameManager.Money >= price)
        {
            if (inventoryManager.AddItem(item))
            {
                SoundFXManager.instance.PlaySoundFXClip(cashOutSound, transform, 1.5f);
                gameManager.Money -= price;
                gameManager.updateMoney();
            }
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
        SoundFXManager.instance.PlaySoundFXClip(buttonPressSound, transform, 1.5f);
        descriptionTxt.text = item.name.ToString();

        statTxt.text = item.Description.ToString();
    }
}
