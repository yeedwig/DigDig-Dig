using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
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
    public int itemCursorIndex; //0 갱도, 1 사다리 오른쪽, 2 사다리 왼쪽, 3 레일, 4 엘베문 아래로, 5 엘베문 위로
    [SerializeField] Sprite[] itemCursorSprite;
    [SerializeField] float cursorFastMoveStartTimerMax; //빠르게 움직이기 시작하는 텀
    [SerializeField] float cursorFastMoveTimerMin; // 빠르게 움직이는 최소 텀
    private float cursorFastMoveInterval; //커서 빠르게 움직이는 텀
    [SerializeField] float cursorFastMoveTimerSubtract; //커서 빠르게 움직이는 텀 줄이기
    private float cursorMoveTimer = 0.0f; 

    //에딧 창 관련
    [SerializeField] Tilemap editTilemap; // 에딧창
    [SerializeField] Tilemap gangTilemap; // 갱도 타일맵
    [SerializeField] Tilemap railTilemap; // 레일 타일맵
    [SerializeField] Tilemap ladderTilemap; // 사다리 타일맵
    [SerializeField] Tilemap elevatorPassageTilemap; // 엘리베이터 통로 타일맵
    [SerializeField] Tilemap editBackground; // 배경
    public bool isEditOn=false; //에딧창 켜져있는가

    //ray,collider
    Collider2D obstacleOnCursor;//커서 위에 땅 있는지 확인
    Collider2D gangOnCursor;//커서 위에 갱도 있는지 확인
    RaycastHit2D hit;

    // 기타
    private int obstacleMask;
    private int gangMask;
    private Dictionary<Vector3Int, GameObject> groundDictionary;
    

    // 타일 및 설치물
    [SerializeField] TileBase gang;
    [SerializeField] TileBase rail;
    [SerializeField] TileBase leftLadder;
    [SerializeField] TileBase rightLadder;
    [SerializeField] TileBase elevatorPassage;
    [SerializeField] GameObject elevatorTop;
    [SerializeField] GameObject elevatorBottom;

    // 엘리베이터
    private Vector3 elevatorTopPos;
    private Vector3 elevatorBottomPos;
    public float elevatorPassageCount = 10;
    private Vector3Int elevatorConstructVec;



    void Start()
    {
        cursorSR = cursor.GetComponent<SpriteRenderer>();
        cursorSR.color = new Color(1, 1, 1, 0.7f);
        obstacleMask = 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Structure");
        gangMask = 1 << LayerMask.NameToLayer("Gang");
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
                itemCursorIndex = 0;
                cursor.SetActive(true);
                editBackground.gameObject.SetActive(true);
                editBackground.transform.position = editTilemap.WorldToCell(player.transform.position);
                cursor.transform.position = editTilemap.WorldToCell(player.transform.position)+new Vector3(0.5f,0.5f,0);
                cursorSR.sprite = itemCursorSprite[itemCursorIndex];
            }
            isEditOn = !isEditOn;
        }
    }

    private void MoveEditCursor()
    {
        if (!Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    cursor.transform.position += new Vector3(-1, 0, 0);
                }
                cursorTimer(-1,0);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    cursor.transform.position += new Vector3(1, 0, 0);
                }
                cursorTimer(1, 0);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    cursor.transform.position += new Vector3(0,1,0);
                }
                cursorTimer(0,1);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    cursor.transform.position += new Vector3(0, -1, 0);
                }
                cursorTimer(0, -1);
            }
            else
            {
                cursorMoveTimer = cursorFastMoveStartTimerMax;
                cursorFastMoveInterval = cursorFastMoveStartTimerMax;
            }
        } 
    }

    private void cursorTimer(int x, int y)
    {
        if (cursorMoveTimer > 0)
        {
            cursorMoveTimer--;
        }
        else
        {
            cursor.transform.position += new Vector3(x, y, 0);
            if (cursorFastMoveInterval > cursorFastMoveTimerMin)
            {
                cursorFastMoveInterval -= cursorFastMoveTimerSubtract;
            }
            cursorMoveTimer = cursorFastMoveInterval;
        }
    }

    private void ChangeItemIndex()
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
                        cursorPosInt = editTilemap.WorldToCell(cursor.transform.position);
                        gangTilemap.SetTile(cursorPosInt, gang);
                        groundDictionary[cursorPosInt].GetComponent<Ground>().gangInstalled = true;
                        break;
                    case 1: //오른쪽 사다리
                        Debug.Log("Test");
                        ladderTilemap.SetTile(editTilemap.WorldToCell(cursor.transform.position), rightLadder);
                        break;
                    case 2: //왼쪽 사다리
                        ladderTilemap.SetTile(editTilemap.WorldToCell(cursor.transform.position), leftLadder);
                        break;
                    case 3://레일
                        railTilemap.SetTile(editTilemap.WorldToCell(cursor.transform.position), rail);
                        break;
                    case 4:
                        GameObject Top = GameObject.Instantiate(elevatorTop);
                        Top.transform.position = cursor.transform.position;
                        hit = Physics2D.Raycast(Top.transform.position, new Vector2(0, -1), elevatorPassageCount, obstacleMask);
                        if(hit.collider!=null && hit.collider.gameObject.tag == "Elevator" && !hit.collider.gameObject.GetComponent<Elevator>().isTop) //tag로 바꿀 생각하기
                        {
                            Top.GetComponent<Elevator>().pair = hit.collider.gameObject;
                            hit.collider.gameObject.GetComponent<Elevator>().pair = Top;
                            elevatorInstall(Top,hit.collider.gameObject);
                        }
                        break;
                    case 5:
                        GameObject Bottom = GameObject.Instantiate(elevatorBottom);
                        Bottom.transform.position = cursor.transform.position;
                        hit = Physics2D.Raycast(Bottom.transform.position, new Vector2(0, 1), elevatorPassageCount, obstacleMask);
                        if (hit.collider != null && hit.collider.gameObject.tag == "Elevator" && hit.collider.gameObject.GetComponent<Elevator>().isTop)
                        {
                            Bottom.GetComponent<Elevator>().pair = hit.collider.gameObject;
                            hit.collider.gameObject.GetComponent<Elevator>().pair = Bottom;
                            elevatorInstall(hit.collider.gameObject,Bottom);
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
        // 0 갱도, 1 오른 사다리, 2 왼 사다리, 3 레일, 4 엘리베이터 아래 문, 5 엘리베이터 위쪽 문
        obstacleOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.4f, obstacleMask);
        gangOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.4f, gangMask);
        if (obstacleOnCursor != null)
        {
            return false;
        }
        else
        {
            if(itemCursorIndex == 0)
            {
                return true;
            }
            if (gangOnCursor == null)
            {
                return false;
            }
            else
            {
                if(itemCursorIndex == 4 ||  itemCursorIndex == 5)
                {
                    return true;
                }
                else
                {
                    if (itemCursorIndex == 1)
                    {
                        hit = Physics2D.Raycast(cursor.transform.position, new Vector2(1, 0), 0.7f, obstacleMask);
                    }
                    else if (itemCursorIndex == 2)
                    {
                        hit = Physics2D.Raycast(cursor.transform.position, new Vector2(-1, 0), 0.7f, obstacleMask);
                    }
                    else if (itemCursorIndex == 3)
                    {
                        hit = Physics2D.Raycast(cursor.transform.position, new Vector2(0, -1), 0.7f, obstacleMask);
                    }
                    if(hit.collider != null)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void elevatorInstall(GameObject top, GameObject bottom)
    {
        top.GetComponent<Elevator>().isConnected = true;
        bottom.GetComponent<Elevator>().isConnected = true;
        Vector3 start=top.transform.position, end=bottom.transform.position+new Vector3(0,1f,0);
        while(start.y > end.y)
        {
            elevatorConstructVec = elevatorPassageTilemap.WorldToCell(end);
            elevatorPassageTilemap.SetTile(elevatorConstructVec, elevatorPassage);
            gangTilemap.SetTile(elevatorConstructVec, gang);
            groundDictionary[elevatorConstructVec].GetComponent<Ground>().gangInstalled = true;
            end.y += 1f;
        }
    }
}
