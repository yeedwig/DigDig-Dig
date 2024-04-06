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
    public Item curItem;
    [SerializeField] private int curToolId;
    [SerializeField] private int curItemType;

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

    //Interaction 관련
    public GameObject ShopUI;
    public bool shopUIOpened;
    public bool shopVisited = false;

    //ladder 관련
    public bool isClimbingLadder = false;
    public bool canClimbLadder;
    RaycastHit2D ladderCheckRay;
    private int structureMask;
    [SerializeField] float ladderSpeed;
    private float yMove;
    private float originalGravity;

    void Start()
    {
        Dead = false;
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        editcontroller = GameObject.Find("Edit").GetComponent<EditController>();
        digManager = GetComponent<DigManager>();
        structureMask = 1 << LayerMask.NameToLayer("Structure");
        originalGravity = rb.gravityScale;

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
            CheckCanClimb();
            GetOnOffLadder();

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
        moveOnladder();
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
        curItemType = toolManager.curToolType;
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
            if(curItem == null)
            {
                Debug.Log("No Tool!");
            }
            else
            {
                if(curItem.itemType == 0) //삽인 경우
                {
                    isDigging = true;
                }
                if (curItem.itemType == 1) //드릴인 경우
                {
                    isDrilling = true;
                }
                if (curItem.itemType == 2) //TNT인 경우
                {
                    isPlacingTNT = true;
                    InstallTNT();
                }
            }
            
            
        }
        else if(Input.GetKeyUp(KeyCode.Q) && isWalking == false)
        {
            if (curItem == null)
            {
                Debug.Log("No Tool!");
            }
            else
            {
                if (curItem.itemType == 0)
                {
                    isDigging = false;
                }
                if (curItem.itemType == 1)
                {
                    isDrilling = false;
                }
                if (curItem.itemType == 2)
                {
                    isPlacingTNT = false;
                }
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


        //InterAction
        if (Input.GetKeyDown(KeyCode.C))
        {
            
            if(shopVisited == true)
            {
                if (shopUIOpened == false)
                {
                    ShopUI.SetActive(true);
                    shopUIOpened = true;
                }
                else
                {
                    ShopUI.SetActive(false);
                    shopUIOpened = false;
                }
            }
            
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
        if(!isClimbingLadder)
        {
            if (facingRight == true && moveDir == -1)
            {
                transform.Rotate(0.0f, 180.0f, 0.0f);
                facingRight = false;
            }

            else if (facingRight == false && moveDir == 1)
            {
                transform.Rotate(0.0f, 180.0f, 0.0f);
                facingRight = true;
            }
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
        if(isDigging == true || isDrilling == true||isEditOn || isClimbingLadder)
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

    private void CheckCanClimb() // 사다리 타기 가능한지 확인
    {
        if (facingRight)
        {
            ladderCheckRay = Physics2D.Raycast(this.gameObject.transform.position, transform.right, 0.2f, structureMask);
        }
        else
        {
            ladderCheckRay = Physics2D.Raycast(this.gameObject.transform.position, -transform.right, 0.2f, structureMask);
        }

        if (ladderCheckRay.collider != null)
        {
            canClimbLadder = true;
        }
        else
        {
            canClimbLadder = false;
        }
    }

    private void GetOnOffLadder() // 나중에 옆에 interaction manager로 옮길지도
    {
        if (canClimbLadder&&Input.GetKeyDown(KeyCode.F))
        {
            if (isClimbingLadder)
            {
                isClimbingLadder = false;
                rb.gravityScale = originalGravity;
            }
            else
            {
                isClimbingLadder= true;
                rb.gravityScale = 0;
            }
        }
    }

    private void moveOnladder()
    {
        yMove = Input.GetAxisRaw("Vertical");
        if(isClimbingLadder)
        {
            rb.velocity = new Vector2(0,yMove*ladderSpeed);
        }
    }

    public void InstallTNT()
    {

        if (curToolId == 22 && curItem.isTool == true && curItem != null)
        {
            GameObject TNT = Instantiate(smallTNTPrefab);
            toolManager.useItem(curSelectedToolSlot);
            TNT.transform.position = this.transform.position;
        }

        if (curToolId == 23 && curItem.isTool == true && curItem != null)
        {
            GameObject TNT = Instantiate(TNTPrefab);
            toolManager.useItem(curSelectedToolSlot);
            TNT.transform.position = this.transform.position;
        }

        
    }

    private void CheckIsEditOn()
    {
        isEditOn = editcontroller.isEditOn;
    }

    private void updateStats(int itemID)
    {
        //Shovel
        if(itemID == 0)
        {
            return;
        }
        if (itemID == 1)
        {
            return;
        }
        if (itemID == 2)
        {
            return;
        }
        if (itemID == 3)
        {
            return;
        }
        if (itemID == 4)
        {
            return;
        }
        if (itemID == 5)
        {
            return;
        }
        if (itemID == 6)
        {
            return;
        }
        if (itemID == 7)
        {
            return;
        }
        if (itemID == 8)
        {
            return;
        }
        if (itemID == 9)
        {
            return;
        }
        if (itemID == 10)
        {
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Shop")
        {
            //Debug.Log("EnterShop");
            shopVisited = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Shop")
        {
            //Debug.Log("LeavingShop");
            shopVisited = false;
            ShopUI.SetActive(false);
            shopUIOpened = false;
        }

    }
}
