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
    private Item curItem;
    [SerializeField] private int curToolId;

    //Items
    //[SerializeField] private GameObject[] Items;

    //Walking
    public bool facingRight = true;
    public float walkSpeed = 0.5f;
    private bool canWalk = true;
    public bool isWalking = false;

    //Jumping
    [SerializeField] private float jumpForce;
    private bool canJump = true;
    private bool isJumping = false;

    //Digging
    public bool canDig = true;
    public bool isDigging = false;

    //Drilling
    private bool canDrill = true;
    public bool isDrilling = false;

    //Placing
    private bool canPlace = true;
    public bool isPlacing = false;

    [SerializeField] private GameObject Tools;

    //Tool UI
    public GameObject InventoryUI;
    public bool inventoryOpened;
    public Image curToolImage;
    public int curSelectedToolSlot;

    //IsGrounded
    private bool isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    //tnt
    [SerializeField] GameObject TNTPrefab;
    [SerializeField] GameObject smallTNTPrefab;
    private bool tntIsBig;
    private bool isPlacingTNT;

    //edit 관련
    private bool isEditOn;
    private EditController editcontroller;


    //Dig 관련
    public DigManager digManager;

    void Start()
    {
        Dead = false;
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        editcontroller = GameObject.Find("Edit").GetComponent<EditController>();
        digManager = GetComponent<DigManager>();

        curItem = toolManager.curItem;
        //curItem.itemType = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if(!isEditOn)
        {
            CurrentToolInput();
            CheckInput();
            Flip();
            //UpdateAnimation();
            CheckTool();

            CheckCanWalk();
            CheckIsWalking();
            CheckCurrentTool();

            //ShowCurrentTool();
        }
     

        CheckIsEditOn(); //Edit 창 켜져있는지 확인

    }

    private void ShowCurrentTool()
    {
        curToolImage.sprite = curItem.image;
    }

    

    void FixedUpdate()
    {
        UpdateAnimation();
        Walk();
    }

    void CheckCurrentTool()
    {
        toolManager.ChangeSelectedSlot(curSelectedToolSlot);
        curItem = toolManager.CheckToolBelt(curSelectedToolSlot);
        if(curItem == null)
        {
            isDigging = false;
            isDrilling = false;
            isPlacing = false;
        }
        curToolId = toolManager.curToolId;
    }

    void CurrentToolInput()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            curSelectedToolSlot = 0;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            curSelectedToolSlot = 1;
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            curSelectedToolSlot = 2;
        }
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            curSelectedToolSlot = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            curSelectedToolSlot = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            curSelectedToolSlot = 5;
        }

    }

    private void CheckInput()
    {
        moveDir = Input.GetAxisRaw("Horizontal");

        //인벤토리
        if(Input.GetKeyDown(KeyCode.I))
        {
            if(inventoryOpened == false)
            {
                InventoryUI.SetActive(true);
                inventoryOpened = true;
            }
            else
            {
                InventoryUI.SetActive(false);
                inventoryOpened = false;
            }    
        }

        //파는 거
        if(Input.GetKeyDown(KeyCode.Q) && isWalking == false)
        {
            if(curItem.itemType == 0) //삽인 경우
            {
                isDigging = true; 
            }
            if(curItem.itemType == 1) //드릴인 경우
            {
                isDrilling = true;
            }
            if(curItem.itemType == 2) //TNT인 경우
            {
                isPlacingTNT = true;
                InstallTNT();
            }
            
        }
        else if(Input.GetKeyUp(KeyCode.Q) && isWalking == false)
        {
            if(curItem.itemType == 0)
            {
                isDigging = false;
            }
            if(curItem.itemType == 1)
            {
                isDrilling = false;
            }
            if (curItem.itemType == 2)
            {
                isPlacingTNT = false;
            }
        }

        //점프
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


        //죽이기
        if (Input.GetKeyDown(KeyCode.R))
        {
            Dead = true;
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
        if(isDigging == true || isDrilling == true||isEditOn)
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
    public void InstallTNT()
    {

        if (curToolId == 20 && curItem.isTool == true && curItem != null)
        {
            GameObject TNT = Instantiate(smallTNTPrefab);
            toolManager.useItem(curSelectedToolSlot);
            TNT.transform.position = this.transform.position;
        }

        if (curToolId == 21)
        {
            GameObject TNT = Instantiate(TNTPrefab);
            TNT.transform.position = this.transform.position;
        }

        
    }

    private void CheckIsEditOn()
    {
        isEditOn = editcontroller.isEditOn;
    }
}
