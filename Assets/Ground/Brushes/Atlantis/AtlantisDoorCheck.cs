using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtlantisDoorCheck : MonoBehaviour
{
    [SerializeField] AtlantisDoor atlantisDoor;
    [SerializeField] AtlantisWaterCheck atlantisWaterCheck;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        atlantisDoor.doorOpened = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    
        if (collision.gameObject.tag == "Player" && atlantisWaterCheck.waterTriggered)
        {
            atlantisDoor.doorOpened = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            atlantisDoor.doorOpened = false;
        }
    }
}
