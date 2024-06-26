using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateCrew : MonoBehaviour
{
    [SerializeField] int mapPieceIndex;
    private PirateManager pirateManager;

    [SerializeField] private bool isShovelGiver;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private Item piratesShovel;

    // Start is called before the first frame update
    void Start()
    {
        pirateManager = FindFirstObjectByType<PirateManager>();
        inventoryManager = FindFirstObjectByType<InventoryManager>();  

    }

    // Update is called once per frame
    void Update()
    {
        Collider2D playerChecked = Physics2D.OverlapCircle(transform.position, 2.0f, LayerMask.GetMask("Player"));
        if (playerChecked != null && Input.GetKeyDown(KeyCode.F))
        {
            pirateManager.mapPieceEarned(mapPieceIndex);
            if(isShovelGiver && !pirateManager.shovelGiven)
            {
                inventoryManager.AddItem(piratesShovel);
                pirateManager.shovelGiven = true;
            }
        }
    }
}
