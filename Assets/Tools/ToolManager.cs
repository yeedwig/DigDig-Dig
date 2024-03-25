using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolManager : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameManager GM;

    public GameObject defaultShovel;

    public SpriteRenderer ToolSp;
    private Animator anim;


    public Item curItem;
    public InventoryItem curToolInvenItem;

    public int curToolType; //0이면 삽, 1이면 드릴, 2이면 TNT, 3이면 Radar
    public int curToolId;
    public float curToolDamage;

    private bool ToolisDrilling;
    private bool ToolisDigging;
    private bool ToolisWalking;

    public int selectedSlot = -1;

    [SerializeField] private InventorySlot[] ToolBeltInventory; //일단은 사이즈 4

    public int skinNr;

    public Skins[] shovelSkins;
    public Skins[] drillSkins;



    public 
    // Start is called before the first frame update
    void Start()
    {
        /*
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        */
        //defaultShovel.SetActive(true);
        //GetComponentInChildren<InventoryItem>().item = defaultShovel;
        //curItem = ToolBeltInventory[0].GetComponentInChildren<InventoryItem>().item; //처음 삽 시작 데미지
    }

    // Update is called once per frame

    private void Update()
    {
        DestroyDamagedItem();
    }
    void LateUpdate()
    {
        //ChangeSkins();
    }

    public bool toolBeltReset()
    {
        for(int i = 0; i < ToolBeltInventory.Length; i++)
        {
            InventorySlot slot = ToolBeltInventory[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot != null)
            {
                GM.Money += itemInSlot.item.price;

                Destroy(itemInSlot.gameObject);
            }
            
        }
        return true;

        //defaultShovel.SetActive(true);

    }

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
    }

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
        curToolInvenItem = ToolBeltInventory[index].GetComponentInChildren<InventoryItem>();
        curItem = curToolInvenItem.item;
        curToolType = curItem.itemType;
        curToolId = curItem.itemId;
        curToolDamage = curItem.damage;
        return curItem;
    }

    private void CheckCurrentToolSkin()
    {
        
    }


    

    private void ChangeSkins()
    {
        if(ToolSp.sprite.name.Contains("MainShovel"))
        {
            string spriteName = ToolSp.sprite.name;
            spriteName = spriteName.Replace("MainShovel_","");
            int spriteNr = int.Parse(spriteName);

            ToolSp.sprite = shovelSkins[skinNr].sprites[spriteNr];
        }
        else
        {
            
        }
        //if Drill
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
