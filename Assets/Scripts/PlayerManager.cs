using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerManager : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sp;
    private Animator anim;
    private float moveDir;

    public bool Dead;

    //Tool
    [SerializeField] private ToolManager toolManager;
    [SerializeField] private int curTool;

    //Items
    [SerializeField] private GameObject[] Items;

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

    [SerializeField] private GameObject Tools;

    //Tool UI
    [SerializeField] private Sprite[] ToolIcons;
    public Image curToolImage;

    //IsGrounded
    private bool isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        curTool = 0;

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

        ShowCurrentTool(curTool);

    }

    private void ShowCurrentTool(int index)
    {
        curToolImage.sprite = ToolIcons[index];
    }

    void FixedUpdate()
    {
        Walk();
    }

    void CheckCurrentTool()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            curTool = toolManager.CheckToolBelt(0);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            curTool = toolManager.CheckToolBelt(1);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            curTool = toolManager.CheckToolBelt(2);
        }
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            curTool = toolManager.CheckToolBelt(3);
        }

    }

    private void CheckInput()
    {
        moveDir = Input.GetAxisRaw("Horizontal");

        if(Input.GetKey(KeyCode.Q) && isWalking == false)
        {
            if(curTool == 0)
            {
                isDigging = true; 
            }
            if(curTool == 1)
            {
                isDrilling = true;
            }
        }
        else if(Input.GetKeyUp(KeyCode.Q) && isWalking == false)
        {
            if(curTool == 0)
            {
                isDigging = false;
            }
            if(curTool == 1)
            {
                isDrilling = false;
            }
        }

        if(Input.GetButtonDown("Jump") && canJump == true)
        {
            isJumping = true;
            canJump = false;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (IsGrounded() && rb.velocity.y <= 0)
        {
            canJump = true;
            isJumping = false;
        }

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
            //여기에 도구 place 하는 기능 넣기
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
            canDig = false;
            canDrill = false;
        }
        else
        {
            isWalking = false;
            canDig = false;
            canDrill = false;
        }
    }

    private bool IsGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void CheckCanJump()
    {
    }

    private void CheckIsJumping()
    {
    }
}
