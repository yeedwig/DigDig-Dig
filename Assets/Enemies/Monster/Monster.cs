using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private bool isSoldierAnt;
    public float attackDamage;
    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.gameObject.GetComponent<Health>().takeDamage(1.0f);
        }
    }*/


    public Vector3 attackOffSet;
    public float attackRange = 1f;
    public LayerMask attackMask;
    public Vector3 pos;

    public void Attack()
    {
        pos = transform.position;
        pos += transform.right * attackOffSet.x;
        pos += transform.up * attackOffSet.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)
        {
            colInfo.GetComponent<Health>().takeDamage(attackDamage);
        }
    }
}
