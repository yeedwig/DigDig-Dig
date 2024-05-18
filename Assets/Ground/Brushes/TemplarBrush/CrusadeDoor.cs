using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrusadeDoor : MonoBehaviour
{
   public GameObject Door;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Door.SetActive(false);
        }
    }
}
