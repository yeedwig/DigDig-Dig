using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] float breathRadius;
    Collider2D waterCollider;
    [SerializeField] float breathMax;
    public float breath;
    private float waterDamageTimer;
    [SerializeField] float waterDamageTimerGap;
    [SerializeField] float waterDamage;

    //Gas Damage
    int gasMask;
    Collider2D gasCollider;
    private float gasDamageTimer;
    [SerializeField] float gasDamageTimerGap;
    [SerializeField] float[] gasDamageArray;
    private int gasDamageLevel;
    private bool gasLevelUp;
    


    [SerializeField] GameObject leftTop, rightBottom;
    private Health health;


    //state ui
    [SerializeField] private GameObject[] gasUI;
    [SerializeField] private GameObject fireUI;
    [SerializeField] private GameObject waterUI;
    [SerializeField] private Text waterPercentage;
    // Start is called before the first frame update
    void Start()
    {
        health = this.GetComponent<Health>();
        lavaMask = LayerMask.GetMask("Lava");
        waterMask = LayerMask.GetMask("Water");
        gasMask = LayerMask.GetMask("Gas");
        breath = breathMax;
    }

    // Update is called once per frame
    void Update()
    {
        GiveLavaDamage();
        GiveWaterDamage();
        GiveGasDamage();
        playerStatus();
        //gasLevel 5레벨만큼 띄우면 됨
        //lavaDamagenum > 0 일때 화상입고 있음
        //breath/breathMax breath < breathMax 일때 띄우면 됨
    }

    private void playerStatus()
    {
        //gas state
        if(gasDamageLevel > 0)
        {
            if(gasDamageLevel == 1)
            {
                gasUI[0].SetActive(true);
            }
            if(gasDamageLevel == 2)
            {
                gasUI[1].SetActive(true);
            }
            if (gasDamageLevel == 3)
            {
                gasUI[2].SetActive(true);
            }
            if (gasDamageLevel == 4)
            {
                gasUI[3].SetActive(true);
            }
            if (gasDamageLevel == 5)
            {
                gasUI[4].SetActive(true);
            }
        }
        else
        {
            gasUI[0].SetActive(false);
            gasUI[1].SetActive(false);
            gasUI[2].SetActive(false);
            gasUI[3].SetActive(false);
            gasUI[4].SetActive(false);

        }

        if(lavaDamageNum > 0)
        {
            //lava
            fireUI.SetActive(true);
        }
        else
        {
            fireUI.SetActive(false);
        }

        if(breath < breathMax)
        {
            waterUI.SetActive(true);
            int percent = (int)(breath / breathMax);
            waterPercentage.text = percent.ToString() + "%";
        }
    }

    //용암에 접촉하면 lavaDamageTotalTimer가 초기화
    //해당 변수가 0이 되기 전까지 계속 데미지를 받음

    private void GiveLavaDamage()
    {
        lavaCollider = Physics2D.OverlapArea(leftTop.transform.position, rightBottom.transform.position, lavaMask);
        //용암에 접촉하고 있는지 확인
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
        if(this.GetComponent<PlayerManager>().Dead == true)
        {
            lavaDamageTimer = 0.1f;
            lavaDamageNum = 0;
        }
    }

    private void GiveWaterDamage()
    {
        //캐릭터 크기 변하면 변경
        waterCollider = Physics2D.OverlapCircle(this.transform.position,breathRadius, waterMask);
        if(waterCollider != null)
        {
            if (breath >= -0.01f)
            {
                breath -= Time.deltaTime;
            }
        }
        else
        {
            if (breath <= breathMax)
            {
                breath += Time.deltaTime;
            }
        }

        if (breath <= 0)
        {
            waterDamageTimer-= Time.deltaTime;
        }
        else
        {
            waterDamageTimer = 0.1f;
        }

        if(waterDamageTimer < 0)
        {
            health.takeDamage(waterDamage);
            waterDamageTimer = waterDamageTimerGap;
        }
    }

    private void GiveGasDamage()
    {
        gasCollider = Physics2D.OverlapArea(leftTop.transform.position, rightBottom.transform.position, gasMask);
        if (gasCollider != null)
        {
            gasDamageTimer-= Time.deltaTime;
            gasLevelUp = true;
        }
        else
        {
            if (gasDamageLevel > 0)
            {
                gasDamageTimer -= Time.deltaTime;
            }
            else
            {
                gasDamageLevel = 0;
                gasDamageTimer = gasDamageTimerGap;
            }
            gasLevelUp = false;
        }

        if(gasDamageTimer < 0)
        {
            health.takeDamage(gasDamageArray[gasDamageLevel]);
            if (gasLevelUp)
            {
                if (gasDamageLevel < gasDamageArray.Length - 1)
                {
                    gasDamageLevel++;
                }
            }
            else
            {
                gasDamageLevel--;
            }
            gasDamageTimer = gasDamageTimerGap;
        }
        if (this.GetComponent<PlayerManager>().Dead == true)
        {
            gasDamageTimer = 0.1f;
            gasDamageLevel = 0;
        }
    }

    


}
