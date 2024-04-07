using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditController : MonoBehaviour
{
    [SerializeField] GameObject player; 

    //에딧 커서
    [SerializeField] GameObject cursor;
    private SpriteRenderer cursorSR;
    private Vector3 cursorPos; //커서 위치 (벡터)
    private Vector3Int cursorPosInt; // 커서 위치 (좌표)
    private int itemCursorIndex; //0 갱도, 1 사다리 오른쪽, 2 사다리 왼쪽, 3 레일, 4 엘베문 아래로, 5 엘베문 위로
    [SerializeField] Sprite[] itemCursorSprite;
    [SerializeField] GameObject[] itemPrefabs;
    [SerializeField] float cursorMoveTimerMax; //커서 빠르게 움직이는 텀
    private float cursorMoveTimer = 0.0f; 

    //에딧 창 관련
    [SerializeField] Tilemap editTilemap; // 에딧창
    [SerializeField] Tilemap editBackground; // 배경
    public bool isEditOn=false; //에딧창 켜져있는가
    private bool canInstall; //설치 가능한가

    //엘리베이터
    private bool startInstallingElevator=false; //엘리베이터 설치 시작
    [SerializeField] TileBase elevatorPassage; //엘리베이터 통로
    private Vector3 elevatorStartPosition;
    private Vector3 elevatorEndPosition;

    //ray,collider
    Collider2D groundOnCursor;
    RaycastHit2D hit;
    private RaycastHit2D leftDiagonal;
    private RaycastHit2D under;
    private RaycastHit2D rightDiagonal;
    private RaycastHit2D up;

    // 기타
    private int groundMask; 
    private Dictionary<Vector3Int, GameObject> groundDictionary;
    private Ground ground; //땅

    void Start()
    {
        cursorSR = cursor.GetComponent<SpriteRenderer>();
        cursorSR.color = new Color(1, 1, 1, 0.7f);
        groundMask = 1 << LayerMask.NameToLayer("Ground");
        groundDictionary = GameObject.Find("GroundDictionary").GetComponent<GroundDictionary>().groundDictionary;
    }

    // Update is called once per frame
    void Update()
    {
        CheckEdit();
        MoveEditCursor();
        ChangeItemIndex();
        InstallBlock();
    }

    private void CheckEdit()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(isEditOn)
            {
                editBackground.gameObject.SetActive(false);
                cursor.SetActive(false);
            }
            else
            {
                resetEdit();
                cursor.SetActive(true);
                editBackground.gameObject.SetActive(true);
                cursor.transform.position = editTilemap.WorldToCell(player.transform.position)+new Vector3(0.5f,0.5f,0);
                cursorSR.sprite = itemCursorSprite[itemCursorIndex];
            }
            isEditOn = !isEditOn;
        }
    }

    private void resetEdit()
    {
        itemCursorIndex = 0;
        startInstallingElevator = false;
    }

    private void MoveEditCursor()
    {
        if (!Input.GetKey(KeyCode.LeftAlt))
        {
            if (startInstallingElevator)
            {
                if (itemCursorIndex == 4)
                {
                    if (Input.GetKey(KeyCode.DownArrow))
                    {
                        if (cursorMoveTimer > 0)
                        {
                            cursorMoveTimer--;
                        }
                        else
                        {
                            under = Physics2D.Raycast(cursor.transform.position, new Vector2(0, -1), 0.7f, groundMask);
                            if (under.collider == null)
                            {
                                cursorPos.y--;
                                cursor.transform.position = cursorPos;
                                cursorMoveTimer = cursorMoveTimerMax;
                                cursorPosInt = editTilemap.WorldToCell(cursor.transform.position);
                                editTilemap.SetTile(cursorPosInt, elevatorPassage);
                            }

                        }
                    }
                    else if (Input.GetKey(KeyCode.UpArrow))
                    {
                        if (cursorMoveTimer > 0)
                        {
                            cursorMoveTimer--;
                        }
                        else
                        {
                            if (cursorPos.y < elevatorStartPosition.y - 0.5f)
                            {
                                cursorPosInt = editTilemap.WorldToCell(cursor.transform.position);
                                editTilemap.SetTile(cursorPosInt, null);
                                cursorPos.y++;
                                cursor.transform.position = cursorPos;
                                cursorMoveTimer = cursorMoveTimerMax;
                            }
                        }
                    }
                    else
                    {
                        cursorMoveTimer = 15.0f;
                    }
                }
                else if(itemCursorIndex == 5)
                {
                    if (Input.GetKey(KeyCode.UpArrow))
                    {
                        if (cursorMoveTimer > 0)
                        {
                            cursorMoveTimer--;
                        }
                        else
                        {
                            up = Physics2D.Raycast(cursor.transform.position, new Vector2(0, 1), 0.7f, groundMask);
                            if (up.collider == null)
                            {
                                cursorPos.y++;
                                cursor.transform.position = cursorPos;
                                cursorMoveTimer = cursorMoveTimerMax;
                                cursorPosInt = editTilemap.WorldToCell(cursor.transform.position);
                                editTilemap.SetTile(cursorPosInt, elevatorPassage);
                            }

                        }
                    }
                    else if (Input.GetKey(KeyCode.DownArrow))
                    {
                        if (cursorMoveTimer > 0)
                        {
                            cursorMoveTimer--;
                        }
                        else
                        {
                            if (cursorPos.y > elevatorStartPosition.y + 0.5f)
                            {
                                cursorPosInt = editTilemap.WorldToCell(cursor.transform.position);
                                editTilemap.SetTile(cursorPosInt, null);
                                cursorPos.y--;
                                cursor.transform.position = cursorPos;
                                cursorMoveTimer = cursorMoveTimerMax;
                            }
                        }
                    }
                    else
                    {
                        cursorMoveTimer = 15.0f;
                    }
                }
                
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    if (cursorMoveTimer > 0)
                    {
                        cursorMoveTimer--;
                    }
                    else
                    {
                        cursorPos.x--;
                        cursor.transform.position = cursorPos;
                        cursorMoveTimer = cursorMoveTimerMax;
                    }

                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    if (cursorMoveTimer > 0)
                    {
                        cursorMoveTimer--;
                    }
                    else
                    {
                        cursorPos.x++;
                        cursor.transform.position = cursorPos;
                        cursorMoveTimer = cursorMoveTimerMax;
                    }

                }
                else if (Input.GetKey(KeyCode.UpArrow))
                {
                    if (cursorMoveTimer > 0)
                    {
                        cursorMoveTimer--;
                    }
                    else
                    {
                        cursorPos.y++;
                        cursor.transform.position = cursorPos;
                        cursorMoveTimer = cursorMoveTimerMax;
                    }

                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    if (cursorMoveTimer > 0)
                    {
                        cursorMoveTimer--;
                    }
                    else
                    {
                        cursorPos.y--;
                        cursor.transform.position = cursorPos;
                        cursorMoveTimer = cursorMoveTimerMax;
                    }
                }
                else
                {
                    cursorMoveTimer = 15.0f;
                }
            }
            
        }
        
    }

    private void ChangeItemIndex()
    {
        if (startInstallingElevator)
        {
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    itemCursorIndex--;
                    if (itemCursorIndex < 0)
                    {
                        itemCursorIndex = 5;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    itemCursorIndex++;
                    if (itemCursorIndex > 5)
                    {
                        itemCursorIndex = 0;
                    }
                }
                cursorSR.sprite = itemCursorSprite[itemCursorIndex];
            }
            // 만약 엘베 문을 설치한 상태라면 통로 sprite로 변경, 거기서는 좌우로 변경 불가능
        }


    }

    private void InstallBlock()
    {
        if (isEditOn && Input.GetKeyDown(KeyCode.Z))
        {
            if (CheckCanInstall())
            {
                switch (itemCursorIndex)
                {
                    case 0: //갱도
                        cursorPosInt = editBackground.WorldToCell(cursor.transform.position);
                        ground = groundDictionary[cursorPosInt].GetComponent<Ground>();
                        ground.gangInstalled = true;
                        ground.ChangeSpriteByCurrentHealth();
                        break;
                    case 1: //오른쪽 사다리
                        cursorPosInt = editBackground.WorldToCell(cursor.transform.position);
                        ground = groundDictionary[cursorPosInt].GetComponent<Ground>();
                        GameObject ladderRight = Instantiate(itemPrefabs[itemCursorIndex]);
                        ladderRight.transform.position = cursorPosInt + new Vector3(0.5f, 0.5f, 0);
                        ground.structureInstalled = true;
                        break;
                    case 2: //왼쪽 사다리
                        cursorPosInt = editBackground.WorldToCell(cursor.transform.position);
                        ground = groundDictionary[cursorPosInt].GetComponent<Ground>();
                        GameObject ladderLeft = Instantiate(itemPrefabs[itemCursorIndex]);
                        ladderLeft.transform.position = cursorPosInt + new Vector3(0.5f, 0.5f, 0);
                        ground.structureInstalled = true;
                        break;
                    case 3://레일
                        cursorPosInt = editBackground.WorldToCell(cursor.transform.position);
                        ground = groundDictionary[cursorPosInt].GetComponent<Ground>();
                        GameObject rail = Instantiate(itemPrefabs[itemCursorIndex]);
                        rail.transform.position = cursorPosInt+new Vector3(0.5f,0.5f,0);
                        ground.structureInstalled = true;
                        break;
                    case 4: //엘리베이터 문 아래로
                        cursorPosInt = editBackground.WorldToCell(cursor.transform.position);
                        ground = groundDictionary[cursorPosInt].GetComponent<Ground>();
                        ground.structureInstalled = true;
                        if (!startInstallingElevator)
                        {
                            GameObject elevatorDoorDown = Instantiate(itemPrefabs[4]);
                            elevatorDoorDown.transform.position = cursorPosInt + new Vector3(0.5f, 0.5f, 0);
                            startInstallingElevator = true;
                            elevatorStartPosition = elevatorDoorDown.transform.position;
                            cursorSR.sprite = itemCursorSprite[5];
                        }
                        else
                        {
                            GameObject elevatorDoor = Instantiate(itemPrefabs[5]);
                            elevatorDoor.transform.position = cursorPosInt + new Vector3(0.5f, 0.5f, 0);
                            startInstallingElevator = false;
                            elevatorEndPosition = elevatorDoor.transform.position;
                            cursorSR.sprite = itemCursorSprite[4];
                            InstallElevatorPassage();
                        }
                        
                        break;
                    case 5:
                        cursorPosInt = editBackground.WorldToCell(cursor.transform.position);
                        ground = groundDictionary[cursorPosInt].GetComponent<Ground>();
                        ground.structureInstalled = true;
                        if (!startInstallingElevator)
                        {
                            GameObject elevatorDoorDown = Instantiate(itemPrefabs[5]);
                            elevatorDoorDown.transform.position = cursorPosInt + new Vector3(0.5f, 0.5f, 0);
                            startInstallingElevator = true;
                            elevatorStartPosition = elevatorDoorDown.transform.position;
                            cursorSR.sprite = itemCursorSprite[4];
                        }
                        else
                        {
                            GameObject elevatorDoor = Instantiate(itemPrefabs[4]);
                            elevatorDoor.transform.position = cursorPosInt + new Vector3(0.5f, 0.5f, 0);
                            startInstallingElevator = false;
                            elevatorEndPosition = elevatorDoor.transform.position;
                            cursorSR.sprite = itemCursorSprite[5];
                            InstallElevatorPassage();
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
    private bool CheckCanInstall()
    {
        canInstall = false;
        
        switch (itemCursorIndex)
        {
            case 0: //갱도
                groundOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.2f, groundMask);
                if (groundOnCursor == null)
                {
                    canInstall = true;
                }
                break;
            case 1: //오른쪽 사다리
                groundOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.2f, groundMask);
                hit = Physics2D.Raycast(cursor.transform.position, new Vector2(1, 0), 0.7f, groundMask);
                cursorPosInt = editBackground.WorldToCell(cursor.transform.position);
                ground = groundDictionary[cursorPosInt].GetComponent<Ground>();
                if (groundOnCursor == null && hit.collider != null && ground.gangInstalled && !ground.structureInstalled)
                {
                    canInstall = true;
                }
                break;
            case 2://왼쪽 사다리
                groundOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.2f, groundMask);
                hit = Physics2D.Raycast(cursor.transform.position, new Vector2(-1, 0), 0.7f, groundMask);
                cursorPosInt = editBackground.WorldToCell(cursor.transform.position);
                ground = groundDictionary[cursorPosInt].GetComponent<Ground>();
                if (groundOnCursor == null && hit.collider != null && ground.gangInstalled && !ground.structureInstalled)
                {
                    canInstall = true;
                }
                break;
            case 3: //레일
                groundOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.2f, groundMask);
                hit = Physics2D.Raycast(cursor.transform.position, new Vector2(0,-1), 0.7f, groundMask);
                cursorPosInt = editBackground.WorldToCell(cursor.transform.position);
                ground = groundDictionary[cursorPosInt].GetComponent<Ground>();
                if (groundOnCursor == null && hit.collider!=null && ground.gangInstalled && !ground.structureInstalled)
                {
                    canInstall = true;
                }
                break;
            case 4: //엘리베이터 문 아래로
                groundOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.2f, groundMask);
                leftDiagonal=Physics2D.Raycast(cursor.transform.position, new Vector2(-0.9f, -1), 1.0f, groundMask); 
                under=Physics2D.Raycast(cursor.transform.position, new Vector2(0, -1), 0.7f, groundMask); 
                rightDiagonal=Physics2D.Raycast(cursor.transform.position, new Vector2(0.9f, -1), 1.0f, groundMask);
                cursorPosInt = editBackground.WorldToCell(cursor.transform.position);
                ground = groundDictionary[cursorPosInt].GetComponent<Ground>();
                if (groundOnCursor == null&&leftDiagonal.collider != null&&rightDiagonal.collider != null && ground.gangInstalled && !ground.structureInstalled)
                {
                    if (startInstallingElevator)
                    {
                        canInstall = true;
                    }
                    else
                    {
                        if (under.collider == null)
                        {
                            canInstall = true;
                        }  
                    }
                }
                break;
            case 5:
                groundOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.2f, groundMask);
                leftDiagonal = Physics2D.Raycast(cursor.transform.position, new Vector2(-0.9f, -1), 1.0f, groundMask);
                up = Physics2D.Raycast(cursor.transform.position, new Vector2(0, 1), 0.7f, groundMask);
                rightDiagonal = Physics2D.Raycast(cursor.transform.position, new Vector2(0.9f, -1), 1.0f, groundMask);
                cursorPosInt = editBackground.WorldToCell(cursor.transform.position);
                ground = groundDictionary[cursorPosInt].GetComponent<Ground>();
                if(groundOnCursor ==null && leftDiagonal.collider != null && rightDiagonal.collider != null && ground.gangInstalled && !ground.structureInstalled)
                {
                    if (!startInstallingElevator)
                    {
                        if(up.collider == null)
                        {
                            canInstall = true;
                        }
                    }
                    else
                    {
                        canInstall = true;
                    }
                }
                break;
            default:
                break;
        }
        return canInstall;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(cursor.transform.position, 0.2f);
        //Gizmos.DrawLine(cursor.transform.position,cursor.transform.position+new Vector3(-1,-1,0));
    }

    private void InstallElevatorPassage()
    {
        
        if (elevatorStartPosition.y > elevatorEndPosition.y)
        {
            Vector3Int startPos = editBackground.WorldToCell(elevatorStartPosition - new Vector3(0, 1, 0));
            Vector3Int endPos = editBackground.WorldToCell(elevatorEndPosition + new Vector3(0, 1, 0));
            while (startPos.y > endPos.y)
            {
                GameObject elevatorPassagePrefab = Instantiate(itemPrefabs[6]);
                elevatorPassagePrefab.transform.position = startPos + new Vector3(0.5f, 0.5f, 0);
                startPos.y--;
            }
        }
        else
        {
            Vector3Int startPos = editBackground.WorldToCell(elevatorStartPosition);
            Vector3Int endPos = editBackground.WorldToCell(elevatorEndPosition + new Vector3(0, 1, 0));
            while (startPos.y < endPos.y)
            {
                GameObject elevatorPassagePrefab = Instantiate(itemPrefabs[6]);
                elevatorPassagePrefab.transform.position = startPos + new Vector3(0.5f, 0.5f, 0);
                startPos.y++;
            }
        }
        //ground 조건 추가하기
        editBackground.ClearAllTiles();

    }
}
