using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] public float MaxHealth;
    public float currentHealth;
    private Animator anim;
    private SpriteRenderer sp;
    public bool isInvulnerable = false;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        currentHealth = MaxHealth;
    }
    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        StartCoroutine(damageEffect());
        //hurt effect

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator damageEffect()
    {
        sp.color = new Color(1, 0, 0, 0.5f);
        yield return new WaitForSeconds(0.4f);
        sp.color = new Color(1, 1, 1, 1);
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    void Die()
    {
        anim.SetBool("isDead", true);
    }
}
