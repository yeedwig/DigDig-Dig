using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdle : StateMachineBehaviour
{
    public float spotRange = 5f;
    Transform player;
    Rigidbody2D rb;
    Enemy enemy;
    public int threshold;
    public int count;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        enemy = animator.GetComponent<Enemy>();
        count = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //enemy.LookAtPlayer();
        Debug.Log(count);
        if(player.GetComponent<PlayerManager>().isDigging)
        {
            count++;
        }
        if (Vector2.Distance(player.position, rb.position) <= spotRange && count >= threshold)
        {
            animator.SetTrigger("Run");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Run");
    }
}
