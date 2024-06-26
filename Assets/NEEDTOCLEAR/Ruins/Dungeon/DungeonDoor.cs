using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDoor : MonoBehaviour
{
    public bool doorOpened;
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("Open", doorOpened);
        //Debug.Log(doorOpened);
    }
    void OpenCloseDoor()
    {

        this.gameObject.GetComponent<BoxCollider2D>().enabled = !doorOpened;
    }
}
