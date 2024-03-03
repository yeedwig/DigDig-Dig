using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sp;
    private Animator anim;
    private float moveDir;

    public bool Dead;

    //Tool
    [SerializeField] private int curTool;

    //Walking
    public bool facingRight = true;
    [SerializeField] private float walkSpeed = 0.5f;
    private bool canWalk = true;
    public bool isWalking = false;

    //Jumping
    [SerializeField] private float jumpForce;
    private bool canJump = true;
    private bool isJumping = false;

    //Digging
    private bool canDig = true;
    public bool isDigging = false;

    //Drilling
    private bool canDrill = true;
    public bool isDrilling = false;

    //Placing
    private bool canPlace = true;
    public bool isPlacing = false;

    // Start is called before the first frame update

    [SerializeField] private GameObject Tools;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        CheckCurrentTool();
        CheckInput();
        Flip();
        UpdateAnimation();
        CheckTool();

        CheckCanWalk();
        CheckIsWalking();
    }

    void FixedUpdate()
    {
        Walk();
    }

    void CheckCurrentTool()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            curTool = 1;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            curTool = 2;  
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            curTool = 3;
        }
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            curTool = 4;
        }
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            curTool = 5;
        }
        if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            curTool = 6;
        }
    }
    private void CheckInput()
    {
        moveDir = Input.GetAxisRaw("Horizontal");
        
        /*
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
        }*/

        if(Input.GetKey(KeyCode.Q))
        {
            if(curTool == 0)
            {

            }
            if(curTool == 1)
            {
                isDigging = true; 
            }
            if(curTool == 2)
            {
                isDrilling = true;
            }
        }
        else if(Input.GetKeyUp(KeyCode.Q))
        {
            if(curTool == 0)
            {

            }
            if(curTool == 1)
            {
                isDigging = false;
            }
            if(curTool == 2)
            {
                isDrilling = false;
            }
        }

        if(Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        isJumping = false;

    }

    void CheckTool()
    {
        if(isDigging == true || isDrilling == true)
        {
            Tools.SetActive(true);
        }
        else
        {
            Tools.SetActive(false);
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
        if ((rb.velocity.x > 0.1f || rb.velocity.x < -0.1f) && isJumping == false)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void CheckCanJump()
    {
    }

    private void CheckIsJumping()
    {
    }
}
