using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    //public Text hpText;
    public float maxHP;
    public float curHP;

    private PlayerManager Player;

    [SerializeField] private float healTimer;
    [SerializeField] private float healTime;

    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        ResetHealth();
        Player = GetComponent<PlayerManager>();
        healTimer = 0;

    }

    void ResetHealth()
    {
        curHP = maxHP;
        healthBar.SetMaxHealth(maxHP);
    }

    public void takeDamage(float damage)
    {
        curHP -= damage;
        healthBar.SetHealth(curHP);
        StartCoroutine(damageRed());
        if(curHP <= 0)
        {
            curHP = 0;
            Player.Dead = true;
        }
    }

    IEnumerator damageRed()
    {
        this.GetComponent<SpriteRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        this.GetComponent<SpriteRenderer>().material.color = Color.white;
    }

    public void Heal(float amount)
    {
        if(curHP + amount <= maxHP)
        {
            curHP += amount;
        }
        else
        {
            curHP = maxHP;
        }
        healthBar.SetHealth(curHP);
    }

    private void HealTimer()
    {
            
        healTimer += Time.deltaTime;
        if(healTimer > healTime)
        {
            healTimer = 0;
            curHP++;
            healthBar.SetHealth(curHP);
        }
             
    }

    private void Update()
    {
        if (this.gameObject.transform.position.y > -1)
        {
            if(curHP < maxHP)
            {
                HealTimer();
            }
            
        }
        
        //hpText.text = curHP.ToString();
    }

}
