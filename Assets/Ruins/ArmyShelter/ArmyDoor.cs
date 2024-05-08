using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyDoor : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item warIsOverMessage;
    public NPC GuardSoldier;
    private bool DoorOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = FindFirstObjectByType<InventoryManager>();    
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D playerChecked = Physics2D.OverlapCircle(transform.position, 2.0f, LayerMask.GetMask("Player"));

        if(playerChecked != null && Input.GetKeyDown(KeyCode.F) && DoorOpen == false)
        {   
            if(inventoryManager.SearchInventory(warIsOverMessage))///비번이 맞으면
            {
                DoorOpen = true;
                OpenDoor();
                GuardSoldier.ChangeChapter();
            }
            
        }        
    }

    public void OpenDoor()
    {
        this.gameObject.SetActive(false);
    }
}
