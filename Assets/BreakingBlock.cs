using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingBlock : MonoBehaviour
{
    [SerializeField] LayerMask layermask;
    [SerializeField] float timerMax;
    public float timer;
    [SerializeField] GameObject target;

    [SerializeField] GameObject n1, n2;
    void Start()
    {
        timer = timerMax;
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D c = Physics2D.OverlapArea(n1.transform.position, n2.transform.position, layermask);
        if(c!=null && c.gameObject==target)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if(timer <= timerMax)
            {
                timer += Time.deltaTime;
            }
        }

        if(timer<0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
