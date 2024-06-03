using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdle : StateMachineBehaviour
{
    public float spotRange = 5f;
    public float getAwayRange = 10f;
    Transform player;
    Rigidbody2D rb;
    Monster enemy;
    public float threshold;
    public float count;

    public bool isRunning;
    public Sprite[] alertImages;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        enemy = animator.GetComponent<Monster>();
        //count = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //enemy.LookAtPlayer();
        //Debug.Log(count);
        if(player.GetComponent<PlayerManager>().isDigging)
        {
            count += Time.deltaTime;
        }
        if(count > threshold * 1 / 4 && count < threshold * 2 / 4)
        {
            enemy.alertMark.GetComponent<SpriteRenderer>().sprite = alertImages[0];
            enemy.alertMark.SetActive(true);
        }
        if(count > threshold* 2/4 && count < threshold* 3/4)
        {
            enemy.alertMark.GetComponent<SpriteRenderer>().sprite = alertImages[1];
        }
        if (Vector2.Distance(player.position, rb.position) <= spotRange && count >= threshold)
        {
            enemy.alertMark.GetComponent<SpriteRenderer>().sprite = alertImages[2];
            animator.SetTrigger("Run");
            enemy.chasing = true;
        }

        else if(Vector2.Distance(player.position, rb.position) >= getAwayRange)
        {
            enemy.chasing = false;
            count = 0;
            enemy.alertMark.SetActive(false);
            enemy.alertMark.GetComponent<SpriteRenderer>().sprite = alertImages[0];
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Run");
    }
}
