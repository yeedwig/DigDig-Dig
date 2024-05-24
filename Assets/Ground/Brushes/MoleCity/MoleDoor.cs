using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleDoor : MonoBehaviour
{
    public bool doorOpened;
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("Open",doorOpened);
    }
    void OpenCloseDoor()
    {
        this.gameObject.SetActive(!doorOpened);
    }
}
