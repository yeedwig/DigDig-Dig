using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sp;
    private Animator anim;
    private float moveDir;

    public bool facingRight = true;
    [SerializeField] private float walkSpeed = 0.5f;
    private bool canWalk = true;
    private bool isWalking = false;


    private bool canDig = true;
    private bool isDigging = false;

    private bool canDrill = true;
    private bool isDrilling = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        Flip();
        UpdateAnimation();

        CheckCanWalk();
        CheckIsWalking();
    }

    void FixedUpdate()
    {
        Walk();
    }

    private void CheckInput()
    {
        moveDir = Input.GetAxisRaw("Horizontal");

        //Digging
        if(Input.GetKey(KeyCode.E))
        {
            isDigging = true;
        }
        else if(Input.GetKeyUp(KeyCode.E))
        {
            isDigging = false;
        }

        //Drilling
        if(Input.GetKey(KeyCode.Q))
        {
            isDrilling = true;
        }
        else if(Input.GetKeyUp(KeyCode.Q))
        {
            isDrilling = false;
        }

    }

    private void Flip()
    {
        if(facingRight == true && moveDir == -1)
        {
            transform.Rotate(0.0f, 180.0f, 0.0f);
            facingRight = false;
        }

        else if(facingRight == false && moveDir == 1)
        {
            transform.Rotate(0.0f, 180.0f, 0.0f);
            facingRight = true;
        }
    }

    private void UpdateAnimation()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isDigging", isDigging);
        anim.SetBool("isDrilling", isDrilling);
    }


    //Walking
    private void Walk()
    {
        if(canWalk)
        {
            rb.velocity = new Vector2(moveDir * walkSpeed, rb.velocity.y);
        }
    }
    
    private void CheckCanWalk()
    {
        if(isDigging == true || isDrilling == true)
        {
            canWalk = false;
        }
        else
        {
            canWalk = true;
        }
    }

    private void CheckIsWalking()
    {
        if (rb.velocity.x > 0.1f || rb.velocity.x < -0.1f)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }
}
