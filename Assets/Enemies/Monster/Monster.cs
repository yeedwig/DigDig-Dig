using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float attackDamage;
    public float groundDigDamage;
    public Vector3 attackOffSet;
    public float attackRange = 1f;
    public LayerMask attackMask;
    public Vector3 pos;
    private Enemy enemy;
    private MonsterIdle monsterIdle;
    private Animator animator;
    Rigidbody2D rb;
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
        enemy = this.GetComponent<Enemy>();
        rb = this.GetComponent<Rigidbody2D>();
        //animator = this.GetComponent<Animator>();
        //monsterIdle = animator.GetComponent<MonsterIdle>();

    }

    private void Update()
    {
        pos = transform.position;
        pos += transform.right * attackOffSet.x;
        pos += transform.up * attackOffSet.y;

        Collider2D groundChecked = Physics2D.OverlapCircle(pos, 1.0f, LayerMask.GetMask("Ground"));
        
        if (groundChecked != null && (rb.velocity.x != 0 || rb.velocity.y != 0))//monsterIdle.isRunning)
        {

            groundChecked.gameObject.GetComponent<Ground>().MonsterDamage(groundDigDamage);


        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.DrawSphere(pos, 1.0f);
    }


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
