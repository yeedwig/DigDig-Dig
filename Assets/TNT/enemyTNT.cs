using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyTNT : MonoBehaviour
{
    public Item TNT;


    private void Update()
    {
        enemyDamage(TNT.range, TNT.damage);
    }
    private void enemyDamage(float range, float damage)
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(this.transform.position, range, LayerMask.GetMask("Enemy"));
        if (collisions != null)
        {
            foreach (Collider2D col in collisions)
            {
                col.gameObject.GetComponent<EnemyHealth>().takeDamage(damage);
            }
        }
        else
            return;
    }
}
