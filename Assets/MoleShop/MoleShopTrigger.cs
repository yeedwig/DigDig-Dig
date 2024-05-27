using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleShopTrigger : MonoBehaviour
{
    [SerializeField] ShopManager shopManager;

    private void Start()
    {
        shopManager = FindFirstObjectByType<ShopManager>(); 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            shopManager.moleShopFound = true;
        }
    }
}
