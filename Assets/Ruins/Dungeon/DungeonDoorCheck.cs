using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDoorCheck : MonoBehaviour
{
    [SerializeField] DungeonDoor moleDoor;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        moleDoor.doorOpened = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            moleDoor.doorOpened = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            moleDoor.doorOpened = false;
        }
    }
}
