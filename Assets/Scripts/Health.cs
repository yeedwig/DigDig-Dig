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

    public float healAmount;

    public AudioClip[] hurtSound;
    public float hurtVolume;
    // Start is called before the first frame update
    void Awake()
    {
        ResetHealth();
        Player = GetComponent<PlayerManager>();
        healTimer = 0;
    }

    public void ResetHealth()
    {
        curHP = maxHP;
        healthBar.SetMaxHealth(maxHP);
    }

    public void takeDamage(float damage)
    {
        if (curHP < 0.001)
        {
            curHP = 0;
            Player.Dead = true;
        }
        else
        {
            curHP -= damage;
            if (curHP < 0.001)
            {
                curHP = 0;
                Player.Dead = true;
            }
            SoundFXManager.instance.PlaySoundFXClip(hurtSound, transform, hurtVolume);
            healthBar.SetHealth(curHP);
            StartCoroutine(damageRed());
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
            curHP+=healAmount;
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
