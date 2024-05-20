using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float ghostDamage;
    public bool canMove = true;
    public Animator anim;

    private void Start()
    {
        canMove = true;
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Health>().takeDamage(ghostDamage);
            anim.SetBool("Dead", true);
            //Destroy(this.gameObject);
        }
        if(collision.gameObject.tag == "Light")
        {
            canMove = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Light")
        {
            canMove = true;
        }
    }

    private void DestroyGhost()
    {
        Destroy(gameObject);
    }
}
