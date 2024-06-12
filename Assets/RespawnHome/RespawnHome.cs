using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnHome : MonoBehaviour
{
    [SerializeField] private GameObject homeOutside;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            homeOutside.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            homeOutside.SetActive(true);
        }
    }
}
