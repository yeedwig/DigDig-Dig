using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class PlayerManager : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sp;
    private Animator anim;
    private float moveDir;
    private float facingDir;
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
    private int jumpLayerMask;

    //tnt
    [SerializeField] GameObject BigTNTPrefab;
    [SerializeField] GameObject middleTNTPrefab;
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
    RaycastHit2D ladderCheckRayTop;
    RaycastHit2D ladderCheckRayBottom;
    [SerializeField] GameObject ladderCheckPosTop;
    [SerializeField] GameObject ladderCheckPosBottom;
    private int structureMask;
    [SerializeField] float ladderSpeed;
    private float yMove;
    private float originalGravity;


    //AudioSource 관련
    public AudioClip[] footSteps;
    public int moveCounter;
    [SerializeField] private int moveSoundLimit;
    public AudioClip[] jumpSound;

    public AudioClip[] interactionSound;
    public AudioClip[] lightSwitchSound;
    public AudioClip[] killSwitchSound;
    public AudioClip[] toolBeltSwitchSound;
    public AudioClip[] ErrorSound;
    //Respawn
    public bool respawning = false;

    //HeadLight 관련
    public GameObject HeadLight;
    public bool headLightIsActive = false;

    //toolbelt 관련
    public GameObject ToolBelt;
    public bool toolBeltIsActive = false;


    //Character 관련
    public CharacterSO curCharacter;
    public int headLightType;

    
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
        jumpLayerMask = 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("ElevatorSub");
        curItem = toolManager.curItem;
        //curItem.itemType = 0;

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(curCharacter.name);
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
            //GetOnOffLadder();

            InterActionRayCast();
            moveOnladder();
            //headLightType = curCharacter.type; //0이면 광부 모자 1이면 손에 드는거 2이면 발광하는거
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
        //CheckQPressed();
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
            //SoundFXManager.instance.PlaySoundFXClip(ErrorSound, transform, 1.5f);
        }
        curToolId = toolManager.curToolId;
        curItemType = toolManager.curToolType;
    }

    void CurrentToolInput()
    {
        if(!respawning)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SoundFXManager.instance.PlaySoundFXClip(toolBeltSwitchSound, transform, 1.5f);
                curSelectedToolSlot = 0;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SoundFXManager.instance.PlaySoundFXClip(toolBeltSwitchSound, transform, 1.5f);
                curSelectedToolSlot = 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SoundFXManager.instance.PlaySoundFXClip(toolBeltSwitchSound, transform, 1.5f);
                curSelectedToolSlot = 2;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SoundFXManager.instance.PlaySoundFXClip(toolBeltSwitchSound, transform, 1.5f);
                curSelectedToolSlot = 3;
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SoundFXManager.instance.PlaySoundFXClip(toolBeltSwitchSound, transform, 1.5f);
                curSelectedToolSlot = 4;
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                SoundFXManager.instance.PlaySoundFXClip(toolBeltSwitchSound, transform, 1.5f);
                curSelectedToolSlot = 5;
            }
        }
        
    }

    private void CheckInput()
    {
        if(!respawning)
        {
            moveDir = Input.GetAxisRaw("Horizontal");
            if(moveDir != 0)
            {
                facingDir = moveDir;
            }

            //인벤토리
            if (Input.GetKeyDown(KeyCode.I))
            {
                SoundFXManager.instance.PlaySoundFXClip(interactionSound, transform, 1.5f);
                if (inventoryOpened == false)
                {
                    InventoryUI.SetActive(true);
                    ToolBelt.SetActive(true);
                    toolBeltIsActive = true;
                    inventoryOpened = true;
                }
                else
                {
                    InventoryUI.SetActive(false);
                    inventoryOpened = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.T)) //toolbelt
            {
                SoundFXManager.instance.PlaySoundFXClip(interactionSound, transform, 1.5f);
                if (toolBeltIsActive)
                {
                    ToolBelt.SetActive(false);
                    toolBeltIsActive = false;
                }
                else
                {
                    ToolBelt.SetActive(true);
                    toolBeltIsActive = true;
                }
            }

            //파는 거
            if (Input.GetKeyDown(KeyCode.Q) && isWalking == false)
            {
                if (curItem == null)
                {
                    Debug.Log("No Tool!");
                    //깡! 거리는 없다는사운드 여기서 플레이
                }
                else
                {
                    if (curItem.itemType == 0) //삽인 경우
                    {
                        isDigging = true;

                        //curItem의 디깅 사운드
                    }
                    if (curItem.itemType == 1) //드릴인 경우
                    {
                        isDrilling = true;
                        //curItem의 드릴 사운드
                    }
                    if (curItem.itemType == 2) //TNT인 경우
                    {
                        isPlacingTNT = true;
                        //설치 사운드
                        InstallTNT();
                    }
                }


            }
            else if (Input.GetKeyUp(KeyCode.Q)) //&& isWalking == false)
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
                        //사운드 플레이 멈춤
                    }
                    if (curItem.itemType == 1)
                    {
                        isDrilling = false;
                        //사운드 멈춤
                    }
                    if (curItem.itemType == 2)
                    {
                        isPlacingTNT = false;
                    }
                }


            }

            //점프
            
            if (Input.GetButtonDown("Jump") && canJump == true)
            {
                isJumping = true;
                canJump = false;
                SoundFXManager.instance.PlaySoundFXClip(jumpSound, transform, 1.5f);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            

            if (IsGrounded() && rb.velocity.y <= 0)
            {
                canJump = true;
                isJumping = false;
            }


            //죽이기
            if (Input.GetKeyDown(KeyCode.P))// 추후에 폭발 버튼 부분
            {
                SoundFXManager.instance.PlaySoundFXClip(killSwitchSound, transform, 2.0f);
                Dead = true;
            }


            //InterAction
            if (Input.GetKeyDown(KeyCode.F))
            {
                //상점과 interaction
                if (shopVisited == true)
                {
                    if (shopUIOpened == false)
                    {
                        SoundFXManager.instance.PlaySoundFXClip(interactionSound, transform, 1.5f);
                        ShopUI.SetActive(true);
                        shopUIOpened = true;
                    }
                    else
                    {
                        SoundFXManager.instance.PlaySoundFXClip(interactionSound, transform, 1.5f);
                        ShopUI.SetActive(false);
                        shopUIOpened = false;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SoundFXManager.instance.PlaySoundFXClip(interactionSound, transform, 1.5f);
                ShopUI.SetActive(false);
                shopUIOpened = false;
                InventoryUI.SetActive(false);
                inventoryOpened = false;
            }

            if(Input.GetKeyDown(KeyCode.R))
            {
                SoundFXManager.instance.PlaySoundFXClip(lightSwitchSound, transform, 1.5f);
                if (headLightIsActive)
                {
              
                    HeadLight.SetActive(false);
                    headLightIsActive = false;
                }
                else
                {
                    HeadLight.SetActive(true);
                    headLightIsActive = true;
                }
            }

            
        }
        

    }

    public void InterActionRayCast()
    {
        Debug.DrawRay(transform.position, new Vector2(facingDir,0) * 0.5f, Color.red, 0);

        RaycastHit2D NPChit = Physics2D.Raycast(transform.position,new Vector2(facingDir,0), 0.5f, LayerMask.GetMask("NPC"));
        RaycastHit2D ButtonHit = Physics2D.Raycast(transform.position, new Vector2(facingDir, 0), 0.5f, LayerMask.GetMask("Button"));

        if (NPChit.collider != null && Input.GetKeyDown(KeyCode.F))
        {

            NPChit.collider.gameObject.GetComponent<NPC>().index += 1;
            Debug.Log("Hit Npc!");
        }
        
        if(ButtonHit.collider != null && Input.GetKeyDown(KeyCode.F))
        {
            ButtonHit.collider.GetComponent<ScientistButton>().TurnOnButton();
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
        anim.SetBool("isClimbing", isClimbingLadder);
    }

    //Sound 위한 타이머들

    /*
    private void CheckQPressed()
    {
        if(isDigging)
        {
            digCounter++;
            if (digCounter > digSoundLimit)
            {
                digCounter = 0;
                SoundFXManager.instance.PlaySoundFXClip(diggingSound, transform, 0.7f);
            }
        }
    }*/


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
            moveCounter++;
            if (moveCounter > moveSoundLimit)
            {
                moveCounter = 0;
                SoundFXManager.instance.PlaySoundFXClip(footSteps, transform, 0.3f);
            }

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
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, jumpLayerMask);
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, jumpLayerMask);
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
            ladderCheckRayTop = Physics2D.Raycast(ladderCheckPosTop.transform.position, transform.right, 0.1f, structureMask);
            ladderCheckRayBottom = Physics2D.Raycast(ladderCheckPosBottom.transform.position, transform.right, 0.1f, structureMask);
        }
        else
        {
            ladderCheckRayTop = Physics2D.Raycast(ladderCheckPosTop.transform.position, -transform.right, 0.1f, structureMask);
            ladderCheckRayBottom = Physics2D.Raycast(ladderCheckPosBottom.transform.position, -transform.right, 0.1f, structureMask);
        }

        if ((ladderCheckRayTop.collider != null && ladderCheckRayTop.collider.gameObject.tag=="Ladder") || (ladderCheckRayBottom.collider != null && ladderCheckRayBottom.collider.gameObject.tag == "Ladder"))
        {
            canClimbLadder = true;
        }
        else
        {
            canClimbLadder = false;
        }
    }

    private void moveOnladder()
    {
        yMove = Input.GetAxisRaw("Vertical");
        // f 누르는 방식
        if (canClimbLadder)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (isClimbingLadder)
                {
                    isClimbingLadder = false;
                    rb.gravityScale = originalGravity;
                }
                else
                {
                    isClimbingLadder = true;
                    rb.gravityScale = 0;
                }
            }
        }
        else
        {
            if (isClimbingLadder)
            {
                isClimbingLadder = false;
                rb.gravityScale = originalGravity;
            }
        }

        if (isClimbingLadder)
        {
            rb.velocity = new Vector2(0, yMove * ladderSpeed);
        }
           
    }

    public void InstallTNT()
    {
        if(curToolId == 21 && curItem.isTool == true && curItem != null)
        {
            GameObject TNT = Instantiate(smallTNTPrefab);
            Rigidbody2D smallRb = TNT.GetComponent<Rigidbody2D>();
            float speed = 2f;
            smallRb.AddForce(transform.up * speed, ForceMode2D.Impulse);
            toolManager.useItem(curSelectedToolSlot);
            TNT.transform.position = this.transform.position;
        }

        if (curToolId == 22 && curItem.isTool == true && curItem != null)
        {
            GameObject TNT = Instantiate(middleTNTPrefab);
            toolManager.useItem(curSelectedToolSlot);
            TNT.transform.position = this.transform.position;
        }

        if (curToolId == 23 && curItem.isTool == true && curItem != null)
        {
            GameObject TNT = Instantiate(BigTNTPrefab);
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
