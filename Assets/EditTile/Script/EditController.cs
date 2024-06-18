using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject gameManager;
    [SerializeField] GameManager GM;
    [SerializeField] PlayerManager Player;
    //에딧 커서
    [SerializeField] GameObject cursor;
    private SpriteRenderer cursorSR;
    private Vector3 cursorPos; //커서 위치 (벡터)
    private Vector3Int cursorPosInt; // 커서 위치 (좌표)
    public int itemCursorIndex; //0 갱도, 1 사다리 오른쪽, 2 사다리 왼쪽, 3 레일, 4 엘베문 아래로, 5 엘베문 위로, 6 제거
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
    Collider2D eraseOnCursor;//커서 위에 지울꺼 있는지 확인
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
    private Vector3Int elevatorConstructVec;

    // 타일 제거
    private GameObject objectToErase;
    private int eraseMask;

    // 설치물 개수 -> 나중에 상점으로 연결
    public int gangNum;
    public int ladderNum;
    public int railNum;
    public int elevatorDoorNum;
    public int elevatorPassageNum;

    private bool isChangingCursor;

    //UI 관련
    [SerializeField] GameObject EditOnWindow;
    [SerializeField] GameObject EditInventory;
    [SerializeField] GameObject ToolBelt;

    //Edit sound
    public AudioClip[] cursorMoveSound;
    public AudioClip[] installSound;
    public AudioClip[] changeStructSound;
    public AudioClip[] editOnSound;
    public AudioClip[] ErrorSound;

    //설치 바운드 관련
    [SerializeField] Camera camera;
    private float originalCameraSize;
    [SerializeField] float editCameraSize;
    [SerializeField] int heightBound;// 세로로 어디까지(절반)
    [SerializeField] int widthBound; // 가로로 어디까지 (절반)
    private Vector3Int playerPos;

    //저장
    [SerializeField] GameObject SaveAndLoad;
    private SaveLoadManager saveLoadManager;
    void Start()
    {
        cursorSR = cursor.GetComponent<SpriteRenderer>();
        cursorSR.color = new Color(1, 1, 1, 0.7f);
        obstacleMask = 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Structure") | 1 << LayerMask.NameToLayer("ElevatorSub");
        eraseMask = 1 << LayerMask.NameToLayer("Structure") | 1 << LayerMask.NameToLayer("ElevatorSub");
        gangMask = 1 << LayerMask.NameToLayer("Gang");
        groundDictionary = GameObject.Find("GroundDictionary").GetComponent<GroundDictionary>().groundDictionary;
        originalCameraSize = camera.fieldOfView;
        saveLoadManager = SaveAndLoad.GetComponent<SaveLoadManager>();
        GM = gameManager.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //GetStructNum();
        CheckEdit();
        MoveEditCursor();
        ChangeItemIndex();
        InstallBlock();
        CheckChangeCursor();
    }

    private void GetStructNum()
    {
        gangNum = GM.GangNum;
        ladderNum = GM.LadderNum;
        railNum = GM.LadderNum;
        elevatorDoorNum = GM.ElevatorDoorNum;
        elevatorPassageNum = GM.ElevatorPassageNum;
    }
    private void CheckChangeCursor()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            isChangingCursor = true;
        }
        else
        {
            isChangingCursor = false;
        }
    }
    private void CheckEdit()
    {
        if(Player.respawning == false)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SoundFXManager.instance.PlaySoundFXClip(editOnSound, transform, 1.0f);
                if (isEditOn)
                {
                    editBackground.gameObject.SetActive(false);
                    cursor.SetActive(false);
                    EditOnWindow.SetActive(false);
                    EditInventory.SetActive(false);
                    ToolBelt.SetActive(true);
                    camera.fieldOfView = originalCameraSize;
                }
                else
                {
                    ToolBelt.SetActive(false);
                    itemCursorIndex = 0;
                    cursor.SetActive(true);
                    editBackground.gameObject.SetActive(true);
                    editBackground.transform.position = editTilemap.WorldToCell(player.transform.position);
                    cursor.transform.position = editTilemap.WorldToCell(player.transform.position) + new Vector3(0.5f, 0.5f, 0);
                    cursorSR.sprite = itemCursorSprite[itemCursorIndex];
                    EditOnWindow.SetActive(true);
                    EditInventory.SetActive(true);
                    camera.fieldOfView = editCameraSize;
                    playerPos = editTilemap.WorldToCell(player.transform.position);
                }
                isEditOn = !isEditOn;
            }
        }
        
    }

    private void MoveEditCursor()
    {
        if (!isChangingCursor && isEditOn)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    cursorPosInt = editTilemap.WorldToCell(cursor.transform.position);
                    if (playerPos.x - (cursorPosInt.x - 1) <= widthBound)
                    {
                        SoundFXManager.instance.PlaySoundFXClip(cursorMoveSound, transform, 1.5f);
                        cursor.transform.position += new Vector3(-1, 0, 0);
                    }
                    
                }
                cursorTimer(-1,0);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                cursorPosInt = editTilemap.WorldToCell(cursor.transform.position);
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if ((cursorPosInt.x + 1) - playerPos.x <= widthBound)
                    {
                        SoundFXManager.instance.PlaySoundFXClip(cursorMoveSound, transform, 1.5f);
                        cursor.transform.position += new Vector3(1, 0, 0);
                    }
                    
                }
                cursorTimer(1, 0);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                cursorPosInt = editTilemap.WorldToCell(cursor.transform.position);
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if ((cursorPosInt.y + 1) - playerPos.y <= heightBound)
                    {
                        SoundFXManager.instance.PlaySoundFXClip(cursorMoveSound, transform, 1.5f);
                        cursor.transform.position += new Vector3(0, 1, 0);
                    }
                        
                }
                cursorTimer(0,1);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                cursorPosInt = editTilemap.WorldToCell(cursor.transform.position);
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (playerPos.y - (cursorPosInt.y - 1) <= heightBound)
                    {
                        SoundFXManager.instance.PlaySoundFXClip(cursorMoveSound, transform, 1.5f);
                        cursor.transform.position += new Vector3(0, -1, 0);
                    }
                        
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
        if (isChangingCursor)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                itemCursorIndex--;
                SoundFXManager.instance.PlaySoundFXClip(changeStructSound, transform, 1.0f);
                if (itemCursorIndex < 0)
                {
                    itemCursorIndex = 6;
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                itemCursorIndex++;
                SoundFXManager.instance.PlaySoundFXClip(changeStructSound, transform, 1.0f);
                if (itemCursorIndex > 6)
                {
                    itemCursorIndex = 0;
                }
            }
            cursorSR.sprite = itemCursorSprite[itemCursorIndex];
        }
    }

    private void InstallBlock()
    {
        if (isEditOn && Input.GetKeyDown(KeyCode.F))
        {
            
            if (CheckCanInstall())
            {
                SoundFXManager.instance.PlaySoundFXClip(installSound, transform, 1.0f);
                switch (itemCursorIndex)
                {
                    case 0: //갱도
                        GangController.instance.CreateGang(editTilemap.WorldToCell(cursor.transform.position));
                        //groundDictionary[cursorPosInt].GetComponent<Ground>().gangInstalled = true; //추후 수정
                        GM.GangNum--;
                        break;
                    case 1: //오른쪽 사다리
                        ladderTilemap.SetTile(editTilemap.WorldToCell(cursor.transform.position), rightLadder);
                        GM.LadderNum--;
                        break;
                    case 2: //왼쪽 사다리
                        ladderTilemap.SetTile(editTilemap.WorldToCell(cursor.transform.position), leftLadder);
                        GM.LadderNum--;
                        break;
                    case 3://레일
                        railTilemap.SetTile(editTilemap.WorldToCell(cursor.transform.position), rail);
                        GM.RailNum--;
                        break;
                    case 4: //엘베 위쪽
                        GameObject Top = GameObject.Instantiate(elevatorTop);
                        GM.ElevatorDoorNum--;
                        Top.transform.position = cursor.transform.position;
                        hit = Physics2D.Raycast(Top.transform.position, new Vector2(0, -1), GM.ElevatorPassageNum+1, obstacleMask);
                        if(hit.collider!=null && hit.collider.gameObject.tag == "Elevator" && !hit.collider.gameObject.GetComponent<Elevator>().isTop) //tag로 바꿀 생각하기
                        {
                            Top.GetComponent<Elevator>().pair = hit.collider.gameObject;
                            hit.collider.gameObject.GetComponent<Elevator>().pair = Top;
                            elevatorInstall(Top,hit.collider.gameObject);
                            //여기 엘베 설치 부분 내일 다시 생각해보기
                        }
                        else
                        {
                            saveLoadManager.topDic.Add(Top, null);
                        }
                        break;
                    case 5://엘베 아래쪽
                        GameObject Bottom = GameObject.Instantiate(elevatorBottom);
                        GM.ElevatorDoorNum--;
                        Bottom.transform.position = cursor.transform.position;
                        hit = Physics2D.Raycast(Bottom.transform.position, new Vector2(0, 1), GM.ElevatorPassageNum + 1, obstacleMask);
                        if (hit.collider != null && hit.collider.gameObject.tag == "Elevator" && hit.collider.gameObject.GetComponent<Elevator>().isTop)
                        {
                            Bottom.GetComponent<Elevator>().pair = hit.collider.gameObject;
                            hit.collider.gameObject.GetComponent<Elevator>().pair = Bottom;
                            elevatorInstall(hit.collider.gameObject,Bottom);
                        }
                        else
                        {
                            saveLoadManager.botDic.Add(Bottom, null);
                        }
                        break;
                    case 6:
                        if (objectToErase.name == "GangTilemap")
                        {
                            GangController.instance.DestroyGang(editTilemap.WorldToCell(cursor.transform.position));
                            //groundDictionary[cursorPosInt].GetComponent<Ground>().gangInstalled = false;
                        }
                        else if(objectToErase.name == "ElevatorPassageTilemap")
                        {
                            //수정 필요할 지도
                            hit = Physics2D.Raycast(cursor.transform.position, new Vector2(0, 1), 100000.0f, obstacleMask);
                            DestroyElevatorPassage(hit.collider.gameObject, hit.collider.gameObject.GetComponent<Elevator>().pair);
                        }
                        else if (objectToErase.tag == "Elevator")
                        {
                            if (objectToErase.GetComponent<Elevator>().pair == null)
                            {
                                if (objectToErase.GetComponent<Elevator>().isTop)
                                {
                                    saveLoadManager.topDic.Remove(objectToErase);
                                }
                                else
                                {
                                    saveLoadManager.botDic.Remove(objectToErase);
                                }
                                Destroy(objectToErase);
                            }
                            else
                            {
                                DestroyElevatorPassage(objectToErase.GetComponent<Elevator>().pair, objectToErase);
                            }
                        }
                        else
                        {
                            if(objectToErase.name == "LadderTilemap")
                            {
                                ladderTilemap.SetTile(editTilemap.WorldToCell(cursor.transform.position), null);
                            }
                            else if(objectToErase.name == "RailTilemap")
                            {
                                railTilemap.SetTile(editTilemap.WorldToCell(cursor.transform.position), null);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                SoundFXManager.instance.PlaySoundFXClip(ErrorSound, transform, 1.0f);
            }
        }
    }
    private bool CheckCanInstall()
    {
        // 0 갱도, 1 오른 사다리, 2 왼 사다리, 3 레일, 4 엘리베이터 아래 문, 5 엘리베이터 위쪽 문
        obstacleOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.4f, obstacleMask);
        gangOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.4f, gangMask);
        if(itemCursorIndex == 6)
        {
            eraseOnCursor=Physics2D.OverlapCircle(cursor.transform.position, 0.4f, eraseMask);
            if (eraseOnCursor != null)
            {
                objectToErase = eraseOnCursor.gameObject;
                return true;
            }
            else if(gangOnCursor != null)
            {
                objectToErase = gangOnCursor.gameObject;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (obstacleOnCursor != null)
            {
                return false;
            }
            else
            {
                if (itemCursorIndex == 0)
                {
                    if (groundDictionary.ContainsKey(editTilemap.WorldToCell(cursor.transform.position))&&GM.GangNum>0&&gangOnCursor==null)
                    {
                        return true; //갱도 설치 가능
                    }               
                }
                if (gangOnCursor == null)
                {
                    return false;
                }
                else
                {
                    if (itemCursorIndex == 4 || itemCursorIndex == 5) //엘리베이터 문 설치
                    {
                        if (elevatorDoorNum > 0) return true;
                        else return false; 
                    }
                    else
                    {
                        if (itemCursorIndex == 1) //사다리 설치
                        {
                            if (GM.LadderNum <= 0) return false;
                            hit = Physics2D.Raycast(cursor.transform.position, new Vector2(1, 0), 0.7f, obstacleMask);
                        }
                        else if (itemCursorIndex == 2)//사다리 설치
                        {
                            if (GM.LadderNum <= 0) return false;
                            hit = Physics2D.Raycast(cursor.transform.position, new Vector2(-1, 0), 0.7f, obstacleMask);
                        }
                        else if (itemCursorIndex == 3)//레일 설치
                        {
                            if (GM.RailNum <= 0) return false;
                            hit = Physics2D.Raycast(cursor.transform.position, new Vector2(0, -1), 0.7f, obstacleMask);
                        }
                        if (hit.collider != null)
                        {
                            return true;
                        }
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
        elevatorTopPos = top.transform.position;
        elevatorBottomPos =bottom.transform.position+new Vector3(0,1f,0);
        while(elevatorTopPos.y > elevatorBottomPos.y)
        {
            elevatorConstructVec = elevatorPassageTilemap.WorldToCell(elevatorBottomPos);
            elevatorPassageTilemap.SetTile(elevatorConstructVec, elevatorPassage);
            GangController.instance.CreateGang(elevatorConstructVec);
            //groundDictionary[elevatorConstructVec].GetComponent<Ground>().gangInstalled = true;
            //elevatorDoorNum--;
            GM.ElevatorPassageNum--;
            elevatorBottomPos.y += 1f;
        }
        if (saveLoadManager.topDic.ContainsKey(top))
        {
            saveLoadManager.topDic[top] = bottom;
        }
        else
        {
            saveLoadManager.topDic.Add(top,bottom);
        }
        if (saveLoadManager.botDic.ContainsKey(bottom))
        {
            saveLoadManager.botDic[bottom] = top;
        }
        else
        {
            saveLoadManager.botDic.Add(bottom, top);
        }
    }

    private void DestroyElevatorPassage(GameObject d1,GameObject d2)
    {
        if (d1.transform.position.y > d2.transform.position.y)
        {
            elevatorTopPos = d1.transform.position;
            elevatorBottomPos = d2.transform.position + new Vector3(0, 1f, 0);
            saveLoadManager.topDic.Remove(d1);
            saveLoadManager.botDic.Remove(d2);
        }
        else
        {
            elevatorTopPos = d2.transform.position;
            elevatorBottomPos = d1.transform.position + new Vector3(0, 1f, 0);
            saveLoadManager.topDic.Remove(d2);
            saveLoadManager.botDic.Remove(d1);
        }
        while (elevatorTopPos.y > elevatorBottomPos.y)
        {
            elevatorConstructVec = elevatorPassageTilemap.WorldToCell(elevatorBottomPos);
            elevatorPassageTilemap.SetTile(elevatorConstructVec, null);
            elevatorBottomPos.y += 1f;
        }
        Destroy(d1);
        Destroy(d2);

    }
}
