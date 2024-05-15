using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidInteraction : MonoBehaviour
{
    //Lava Damage
    int lavaMask;
    Collider2D lavaCollider;
    private float lavaDamageTimer;
    [SerializeField] float lavaDamageTimerGap;
    [SerializeField] float lavaDamage;



    [SerializeField] GameObject leftTop, rightBottom;
    private Health health;
    // Start is called before the first frame update
    void Start()
    {
        health = this.GetComponent<Health>();
        lavaMask = LayerMask.GetMask("Lava");
    }

    // Update is called once per frame
    void Update()
    {
        GiveLavaDamage();
    }

    private void GiveLavaDamage()
    {
        lavaCollider = Physics2D.OverlapArea(leftTop.transform.position, rightBottom.transform.position, lavaMask);
        if(lavaCollider != null)
        {
            lavaDamageTimer -= Time.deltaTime;
        }
        else
        {
            lavaDamageTimer = 0.1f;
        }
        if(lavaDamageTimer < 0)
        {
            health.takeDamage(lavaDamage);
            lavaDamageTimer = lavaDamageTimerGap;
        }
    }

    
}
