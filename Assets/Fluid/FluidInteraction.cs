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
    [SerializeField] int lavaDamageNumMax;
    private int lavaDamageNum;

    //Water Damage
    int waterMask;
    Collider2D waterCollider;
    [SerializeField] float breathMax;
    private float breath;
    private float waterDamageTimer;
    [SerializeField] float waterDamageTimerGap;
    [SerializeField] float waterDamage;


    [SerializeField] GameObject leftTop, rightBottom;
    private Health health;
    // Start is called before the first frame update
    void Start()
    {
        health = this.GetComponent<Health>();
        lavaMask = LayerMask.GetMask("Lava");
        waterMask = LayerMask.GetMask("Water");
    }

    // Update is called once per frame
    void Update()
    {
        GiveLavaDamage();
        GiveWaterDamage();
    }

    //��Ͽ� �����ϸ� lavaDamageTotalTimer�� �ʱ�ȭ
    //�ش� ������ 0�� �Ǳ� ������ ��� �������� ����

    private void GiveLavaDamage()
    {
        lavaCollider = Physics2D.OverlapArea(leftTop.transform.position, rightBottom.transform.position, lavaMask);
        //��Ͽ� �����ϰ� �ִ��� Ȯ��
        if(lavaCollider != null)
        {
            lavaDamageTimer -= Time.deltaTime;
            lavaDamageNum = lavaDamageNumMax;
        }
        else
        {
            if(lavaDamageNum>0)
            {
                lavaDamageTimer -= Time.deltaTime;
            }
            else
            {
                lavaDamageTimer = 0.1f;
            }
        }

        if(lavaDamageTimer < 0)
        {
            health.takeDamage(lavaDamage);
            lavaDamageNum--;
            lavaDamageTimer = lavaDamageTimerGap;
        }
    }

    private void GiveWaterDamage()
    {
        waterCollider = Physics2D.OverlapArea(leftTop.transform.position, rightBottom.transform.position, waterMask);
        if(waterCollider != null)
        {
            
        }
    }

    
}
