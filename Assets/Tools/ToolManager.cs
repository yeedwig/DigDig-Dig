using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ToolManager : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameManager GM;

    public SpriteRenderer ToolSp;
    private Animator anim;


    public Item curItem;
    public InventoryItem curToolInvenItem;

    public int curToolType; //0이면 삽, 1이면 드릴, 2이면 TNT, 3이면 Radar
    public int curToolId;
    public float curToolDamage;
    public int curToolEfficiency;
    public int curSelectedSlot;

    private bool ToolisDrilling;
    private bool ToolisDigging;
    private bool ToolisWalking;

    public int selectedSlot = -1;

    public InventorySlot[] ToolBeltInventory; 

    //public int skinNr;

    public Skins[] shovelSkins;
    public Skins[] drillSkins;

    //reset
    public GameObject inventoryItemPrefab;
    

    public InventoryManager inventoryManager;
    public Item defaultShovel;

    public float durabilityDamage;

    public AudioClip[] ErrorSound;

    // Start is called before the first frame update
    void Start()
    {
        inventoryManager.SpawnNewItem(defaultShovel, ToolBeltInventory[0]);
    }

    // Update is called once per frame

    private void Update()
    {
        //DestroyDamagedItem();
    }
    void LateUpdate()
    {
        ChangeSkins(curToolId);
    }

    public bool toolBeltReset()
    {
        //reset all objects in toolbelt and add money according to price
        for(int i = 0; i < ToolBeltInventory.Length; i++)
        {
            InventorySlot slot = ToolBeltInventory[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot != null)
            {
                //GM.MoneyAdded(itemInSlot.item.price);

                Destroy(itemInSlot.gameObject);
            }
            
        }

        //inventoryManager.SpawnNewItem(defaultShovel, ToolBeltInventory[0]);
        
        return true;

    }

    /*
    public void DestroyDamagedItem()
    {
        for (int i = 0; i < ToolBeltInventory.Length; i++)
        {
            InventorySlot slot = ToolBeltInventory[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                if(itemInSlot.Durability < 0)
                    Destroy(itemInSlot.gameObject);
            }

        }
    }*/

    public void ChangeSelectedSlot(int newValue)
    {
        if(selectedSlot >= 0)
        {
            ToolBeltInventory[selectedSlot].Deselect(); 
        }

        ToolBeltInventory[newValue].Select();
        selectedSlot = newValue;
    }

    public Item CheckToolBelt(int index)
    {
        if (ToolBeltInventory[index].GetComponentInChildren<InventoryItem>() != null)
        {
            curSelectedSlot = index;
            curToolInvenItem = ToolBeltInventory[index].GetComponentInChildren<InventoryItem>();
            curItem = curToolInvenItem.item;
            curToolType = curItem.itemType;
            curToolId = curItem.itemId;
            curToolDamage = curItem.damage;
            curToolEfficiency = curItem.efficiency;
            return curItem;
        }
        else
        {
            //SoundFXManager.instance.PlaySoundFXClip(ErrorSound, transform, 1.5f);
            return null;
        }
    }

    public void useItem(int index)
    {
        //tnt나 레이더 같이 카운트가 있는 경우
        InventorySlot useItemSlot = ToolBeltInventory[index];
        InventoryItem useInventoryItem = useItemSlot.GetComponentInChildren<InventoryItem>();
        if(useInventoryItem != null)
        {
            //삽이나 드릴인 경우
            if(curToolType >= 0 && curToolType <=1)
            {
                useInventoryItem.Damage(durabilityDamage);
                if(useInventoryItem.Durability <= 0)
                {
                    Destroy(useInventoryItem.gameObject);
                    curItem = null;
                }
                else
                {
                    //useInventoryItem.RefreshCount();
                }
            }

            //TNT나 레이더인경우
            if(curToolType >= 2 && curToolType <=3)
            {
                Debug.Log("Count -1");
                useInventoryItem.count--;
                if (useInventoryItem.count <= 0)
                {
                    Destroy(useInventoryItem.gameObject);
                }
                else
                {
                    useInventoryItem.RefreshCount();
                }
            }
            
        }
    }



    

    private void ChangeSkins(int skinNr)
    {
        if(ToolSp.sprite != null)
        {
            if (ToolSp.sprite.name.Contains("MainShovel"))
            {
                string spriteName = ToolSp.sprite.name;
                spriteName = spriteName.Replace("MainShovel_", "");
                int spriteNr = int.Parse(spriteName);

                ToolSp.sprite = shovelSkins[skinNr].sprites[spriteNr];
            }

            else if (ToolSp.sprite.name.Contains("MainDrill"))
            {
                string spriteName = ToolSp.sprite.name;
                spriteName = spriteName.Replace("MainDrill_", "");
                int spriteNr = int.Parse(spriteName);

                ToolSp.sprite = drillSkins[skinNr-11].sprites[spriteNr];

            }

            else
            {
                return;
            }
        }
        
    }

    /*
    private void UpdateAnimation()
    {
        anim.SetBool("isDrilling", ToolisDrilling);
        anim.SetBool("isDigging", ToolisDigging);
        anim.SetBool("isWalking", ToolisWalking);
    }*/


    [System.Serializable] // inspetor에서 보이게 하는 기능
    public struct Skins
    {
        public Sprite[] sprites;
    }
}
