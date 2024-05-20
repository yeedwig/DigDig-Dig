using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameTrap : MonoBehaviour
{
    private Animator anim;
    public float damage;
    private void Start()
    {
        anim = GetComponent<Animator>();   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            anim.SetTrigger("Scatter");
            collision.GetComponent<Health>().takeDamage(damage);
        }
    }

    private void ScatterDestroy()
    {
        Destroy(this.gameObject);
    }
    
}
