using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float attackDamage;
    public float groundDigDamage;
    public Vector3 groundOffSet;
    public float attackRange = 1f;
    public LayerMask attackMask;
    public Vector3 pos;
    private Enemy enemy;
    private MonsterIdle monsterIdle;
    private Animator animator;
    public float groundRadius;
    Rigidbody2D rb;

    public bool chasing;
    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.gameObject.GetComponent<Health>().takeDamage(1.0f);
        }
    }*/
    private void Start()
    {
        chasing = false;
        enemy = this.GetComponent<Enemy>();
        rb = this.GetComponent<Rigidbody2D>();
        //animator = this.GetComponent<Animator>();
        //monsterIdle = animator.GetComponent<MonsterIdle>();

    }

    private void Update()
    {
        pos = transform.position;
        pos += transform.right * groundOffSet.x;
        pos += transform.up * groundOffSet.y;

        Collider2D groundChecked = Physics2D.OverlapCircle(pos, groundRadius, LayerMask.GetMask("Ground"));
        
        if (groundChecked != null && chasing)
        {

            groundChecked.gameObject.GetComponent<Ground>().MonsterDamage(groundDigDamage);


        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.DrawSphere(pos, groundRadius);
    }


    public void Attack()
    {
        pos = transform.position;
        pos += transform.right * groundOffSet.x;
        pos += transform.up * groundOffSet.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)
        {
            colInfo.GetComponent<Health>().takeDamage(attackDamage);
        }
    }
}
