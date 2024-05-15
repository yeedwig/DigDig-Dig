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



    [SerializeField] GameObject leftTop, rightBottom;
    // Start is called before the first frame update
    void Start()
    {
        lavaMask = LayerMask.GetMask("Lava");
    }

    // Update is called once per frame
    void Update()
    {
        LavaDamage();
    }

    private void LavaDamage()
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
            //데미지 들어감
            Debug.Log("용암 데미지");
            lavaDamageTimer = lavaDamageTimerGap;
        }
    }

    
}
