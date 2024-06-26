using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoleIdSlot : MonoBehaviour
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
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void BuyItem()
    {
        if (gameManager.Money >= price)
        {
            SoundFXManager.instance.PlaySoundFXClip(cashOutSound, transform, 1.5f);
            gameManager.Money -= price;
            gameManager.hasMoleId = true;
            gameManager.updateMoney();
            Destroy(this.gameObject);
            //돈빠져나가는 애니메이션
        }
    }

    public void ShowDescription()
    {
        //Show Item Description
        SoundFXManager.instance.PlaySoundFXClip(buttonPressSound, transform, 1.5f);
        descriptionTxt.text = item.name.ToString() + " : " + "\n" + item.Description.ToString();

        statTxt.text = "Damage : " + item.damage.ToString() + "\n" + "Durability : " + item.durability.ToString() + "\n" + "Selling Price : " + item.price.ToString();
    }
}
