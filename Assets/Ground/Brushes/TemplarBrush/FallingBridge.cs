using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FallingBridge : MonoBehaviour
{
    private Animator anim;
    private void Start()
    {
       anim = GetComponent<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            anim.SetTrigger("Collapse");   
        }
    }

    private void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
