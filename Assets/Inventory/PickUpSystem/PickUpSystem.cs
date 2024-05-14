using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField]
    private InventoryManager inventoryManager;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private GameObject AlertMessage;
    [SerializeField] private Text AlertText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickableItem PickUpItem = collision.GetComponent<PickableItem>();
        if (PickUpItem != null)
        {
            inventoryManager.AddItem(PickUpItem.item);
            if(PickUpItem.item.isTool)
            {
                if (PickUpItem.item.itemId == 1 && PickUpItem.item.isTool) //Ant Nest Found
                {
                    gameManager.AntNestFound = true;
                    AlertText.text = "The Sound Of Treasure Has Been Spread to The Ant Nest";
                }

                if (PickUpItem.item.itemId == 2 && PickUpItem.item.isTool) //Military Found
                {
                    gameManager.ArmyTrenchFound = true;
                    AlertText.text = "The Sound Of Treasure Has Been Spread to The Trench";
                }

                if (PickUpItem.item.itemId == 3 && PickUpItem.item.isTool) //Crusade
                {
                    gameManager.CrusadeFound = true;
                    AlertText.text = "The Sound Of Treasure Has Been Spread to the Crusade";
                }

                if (PickUpItem.item.itemId == 4 && PickUpItem.item.isTool) //Catacomb
                {
                    gameManager.CatacombFound = true;
                    AlertText.text = "The Sound Of Treasure Has Been Spread to Death";
                }

                if (PickUpItem.item.itemId == 5 && PickUpItem.item.isTool) //Tribe Found
                {
                    gameManager.UndergroundTribeFound = true;
                    AlertText.text = "The Sound Of Treasure Has Been Spread to The Ancient Civilization";
                }

                if (PickUpItem.item.itemId == 6 && PickUpItem.item.isTool) //Atlantis Found
                {
                    gameManager.AtlantisFound = true;
                    AlertText.text = "The Sound Of Treasure Has Been Spread to Poseidon's Kingdom";
                }

                if (PickUpItem.item.itemId == 7 && PickUpItem.item.isTool) //Lost World
                {
                    gameManager.LostWorldFound = true;
                    AlertText.text = "The Sound Of Treasure Has Been Spread to A Place Trapped in Time";
                }

                if (PickUpItem.item.itemId == 8 && PickUpItem.item.isTool) //Golden City
                {
                    gameManager.EldoradoFound = true;
                    AlertText.text = "The Sound Of Treasure Has Been Spread to The Golden Kingdom";
                }

                if (PickUpItem.item.itemId == 9 && PickUpItem.item.isTool) // Pirates
                {
                    gameManager.PiratesMet = true;
                    AlertText.text = "The Sound Of Treasure Has Been Spread to The Pirates";
                }

                if (PickUpItem.item.itemId == 10 && PickUpItem.item.isTool) // Mad Scientist
                {
                    gameManager.MadScientistLabFound = true;
                    AlertText.text = "A Gift Has Been Given By The Lonely Scholar";
                }
            }
            else if(PickUpItem.item.isKey)
            {
                AlertText.text = "Key Found!";
            }
            else
            {
                AlertText.text = "Item Added!";
            }
            
            StartCoroutine(MessageTimer());
            PickUpItem.DestroyItem();
        }

        IEnumerator MessageTimer()
        {
            AlertMessage.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            AlertMessage.SetActive(false);
        }

    }
}


