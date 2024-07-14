using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditController : MonoBehaviour
{
    //외부에서 불러올 객체들
    [SerializeField] GameObject player;
    [SerializeField] GameObject gameManager;
    private GameManager GM;
    private PlayerManager PM;

    //에딧 커서 관련
    [SerializeField] GameObject cursor; //커서 객체
    private SpriteRenderer cursorSR; //커서 스프라이트 렌더러
    public int itemCursorIndex; //0 갱도, 1 사다리 오른쪽, 2 사다리 왼쪽, 3 레일, 4 엘베문 아래로, 5 엘베문 위로, 6 제거
    [SerializeField] Sprite[] itemCursorSprite; //커서 인덱스 순서로 스프라이트 넣기

    //에딧 창 관련
    [SerializeField] Tilemap editTilemap; // 에딧창
    [SerializeField] Tilemap gangTilemap; // 갱도 타일맵
    [SerializeField] Tilemap railTilemap; // 레일 타일맵
    [SerializeField] Tilemap ladderTilemap; // 사다리 타일맵
    [SerializeField] Tilemap elevatorPassageTilemap; // 엘리베이터 통로 타일맵
    [SerializeField] Tilemap editBackground; // 배경
    public bool isEditOn=false; //에딧창 켜져있는가

    //설치 관련
    private int obstacleMask; //설치시 장애물 있는지 확인
    private int gangMask; //설치시 갱도 있는지 확인
    private int groundMask; //설치시 땅 있는지 확인
    RaycastHit2D hit; //엘리베이터 짝 확인시 사용

    // 기타
    private Dictionary<Vector3Int, GameObject> groundDictionary;
    
    // 타일 및 설치물
    [SerializeField] TileBase gang;
    [SerializeField] TileBase rail;
    [SerializeField] TileBase leftLadder;
    [SerializeField] TileBase rightLadder;
    [SerializeField] TileBase elevatorPassage;
    [SerializeField] GameObject elevatorTop;
    [SerializeField] GameObject elevatorBottom;

    // 타일 제거
    private GameObject objectToErase; //제거할 객체
    private int eraseMask; //제거할 것이 있는지 확인

    //UI 관련
    [SerializeField] GameObject EditOnWindow; //Edit 정보 UI
    [SerializeField] GameObject EditInventory; //Edit 인벤 UI
    [SerializeField] GameObject ToolBelt; //ToolBelt

    // 엘리베이터
    private Vector3 elevatorTopPos;
    private Vector3 elevatorBottomPos;
    private Vector3Int elevatorConstructVec;

    //Edit sound
    public AudioClip[] cursorMoveSound;
    public AudioClip[] installSound;
    public AudioClip[] changeStructSound;
    public AudioClip[] editOnSound;
    public AudioClip[] ErrorSound;

    //설치 바운드 관련
    [SerializeField] Camera camera; //카메라 객체
    public float originalCameraSize; //기존 카메라 크기
    [SerializeField] float editCameraSize; //Edit 전환시 카메라 크기 변환
    [SerializeField] int heightBound;// 세로로 어디까지(절반)
    [SerializeField] int widthBound; // 가로로 어디까지 (절반)

    //저장
    [SerializeField] GameObject SaveAndLoad;
    private SaveLoadManager saveLoadManager;
    void Start()
    {
        GM = gameManager.GetComponent<GameManager>();
        PM = player.GetComponent<PlayerManager>();
        cursorSR = cursor.GetComponent<SpriteRenderer>();
        obstacleMask = 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Structure");
        eraseMask = 1 << LayerMask.NameToLayer("Structure");
        gangMask = 1 << LayerMask.NameToLayer("Gang");
        groundMask = 1 << LayerMask.NameToLayer("Ground");
        groundDictionary = GameObject.Find("GroundDictionary").GetComponent<GroundDictionary>().groundDictionary;
        originalCameraSize = camera.fieldOfView;
        saveLoadManager = SaveAndLoad.GetComponent<SaveLoadManager>();
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
        if(PM.respawning == false)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SoundFXManager.instance.PlaySoundFXClip(editOnSound, transform, 1.0f);
                if (isEditOn)
                {
                    //에딧창 꺼질때
                    editBackground.gameObject.SetActive(false);
                    cursor.SetActive(false);
                    EditOnWindow.SetActive(false);
                    EditInventory.SetActive(false);
                    ToolBelt.SetActive(true);
                    camera.fieldOfView = originalCameraSize;
                }
                else
                {
                    //에딧창 켜질 때
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
                }
                isEditOn = !isEditOn;
            }
        }
    }
    private void MoveEditCursor() //커서 움직이기
    {
        if (!Input.GetKey(KeyCode.LeftAlt) && isEditOn)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) //왼쪽 이동
            {
                if (editTilemap.WorldToCell(player.transform.position).x - (editTilemap.WorldToCell(cursor.transform.position).x - 1) <= widthBound)
                {
                    SoundFXManager.instance.PlaySoundFXClip(cursorMoveSound, transform, 1.5f);
                    cursor.transform.position += new Vector3(-1, 0, 0);
                }  
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow)) //우측 이동
            {
                if ((editTilemap.WorldToCell(cursor.transform.position).x + 1) - editTilemap.WorldToCell(player.transform.position).x <= widthBound)
                {
                    SoundFXManager.instance.PlaySoundFXClip(cursorMoveSound, transform, 1.5f);
                    cursor.transform.position += new Vector3(1, 0, 0);
                } 
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow)) //위로 이동
            {
                if ((editTilemap.WorldToCell(cursor.transform.position).y + 1) - editTilemap.WorldToCell(player.transform.position).y <= heightBound)
                {
                    SoundFXManager.instance.PlaySoundFXClip(cursorMoveSound, transform, 1.5f);
                    cursor.transform.position += new Vector3(0, 1, 0);
                }      
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) //아래로 이동
            {
                if (editTilemap.WorldToCell(player.transform.position).y - (editTilemap.WorldToCell(cursor.transform.position).y - 1) <= heightBound)
                {
                    SoundFXManager.instance.PlaySoundFXClip(cursorMoveSound, transform, 1.5f);
                    cursor.transform.position += new Vector3(0, -1, 0);
                }      
            }
        } 
    }

    private void ChangeItemIndex() //커서 변환
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) //커서 종류 왼쪽으로
            {
                itemCursorIndex--;
                SoundFXManager.instance.PlaySoundFXClip(changeStructSound, transform, 1.0f);
                if (itemCursorIndex < 0)
                {
                    itemCursorIndex = itemCursorSprite.Length-1;
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow)) //커서 종류 오른쪽으로
            {
                itemCursorIndex++;
                SoundFXManager.instance.PlaySoundFXClip(changeStructSound, transform, 1.0f);
                if (itemCursorIndex > itemCursorSprite.Length - 1)
                {
                    itemCursorIndex = 0;
                }
            }
            cursorSR.sprite = itemCursorSprite[itemCursorIndex];
        }
    }

    private void InstallBlock() //블록 설치
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
                        GM.ElevatorDoorNum--;
                        GameObject Top = GameObject.Instantiate(elevatorTop);
                        Top.transform.position = cursor.transform.position;
                        hit = Physics2D.Raycast(Top.transform.position, new Vector2(0, -1), GM.ElevatorPassageNum + 1, obstacleMask);
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
                        GM.ElevatorDoorNum--;
                        GameObject Bottom = GameObject.Instantiate(elevatorBottom);
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
        Collider2D obstacleOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.4f, obstacleMask);
        Collider2D gangOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.4f, gangMask);
        if (itemCursorIndex == 6) //제거
        {
            Collider2D eraseOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.4f, eraseMask);
            if (eraseOnCursor != null)
            {
                objectToErase = eraseOnCursor.gameObject;
                return true;
            }
            else if (gangOnCursor != null)
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
            if (itemCursorIndex == 0)
            {
                if (groundDictionary.ContainsKey(editTilemap.WorldToCell(cursor.transform.position)) && GM.GangNum > 0 && gangOnCursor == null && obstacleOnCursor == null) return true;
            }
            else if (itemCursorIndex == 1)
            {
                hit = Physics2D.Raycast(cursor.transform.position, new Vector2(1, 0), 0.7f, groundMask);
                if (GM.LadderNum > 0 && hit.collider != null && gangOnCursor != null && obstacleOnCursor == null) return true;
            }
            else if (itemCursorIndex == 2)
            {
                hit = Physics2D.Raycast(cursor.transform.position, new Vector2(-1, 0), 0.7f, groundMask);
                if (GM.LadderNum > 0 && hit.collider != null && gangOnCursor != null && obstacleOnCursor == null) return true;
            }
            else if (itemCursorIndex == 3)
            {
                hit = Physics2D.Raycast(cursor.transform.position, new Vector2(0, -1), 0.7f, groundMask);
                if (GM.RailNum > 0 && hit.collider != null && gangOnCursor != null && obstacleOnCursor == null) return true;
            }
            else if (itemCursorIndex == 4)
            {
                if (GM.ElevatorDoorNum > 0 && gangOnCursor != null && obstacleOnCursor == null) return true;
            }
            else if (itemCursorIndex == 5)
            {
                if (GM.ElevatorDoorNum > 0 && gangOnCursor != null && obstacleOnCursor == null) return true;
            }

            return false;
        }
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
