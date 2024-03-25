using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerManager Player;
    public InventoryManager inventoryManager;
    public ToolManager toolManager;
    public CharacterManager characterManager;

    public int Money;
    public Text moneyText;

    private void PlayerDead()
    {
        //reset inventory
        bool inventoryReset = inventoryManager.EmptyAllObjects();
        //reset toolbelt
        bool toolBeltReset = toolManager.toolBeltReset();
        //resetlocation
        //reset character
        bool characterReset = characterManager.resetCharacter();

        if(inventoryReset == true && characterReset == true && toolBeltReset == true)
        {
            Player.Dead = false;
        }
    }   
    private void Start()
    {
        Money = 0;
    }

    private void Update()
    {
        if(Player.Dead == true)
        {
            PlayerDead();
            moneyText.text = Money.ToString();
        }
        //moneyText.text = Money.ToString();  
    }

}
