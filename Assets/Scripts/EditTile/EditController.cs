using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditController : MonoBehaviour
{
    //�ܺο��� �ҷ��� ��ü��
    [SerializeField] GameObject player;
    [SerializeField] GameObject gameManager;
    private GameManager GM;
    private PlayerManager PM;

    //���� Ŀ�� ����
    [SerializeField] GameObject cursor; //Ŀ�� ��ü
    private SpriteRenderer cursorSR; //Ŀ�� ��������Ʈ ������
    public int itemCursorIndex; //0 ����, 1 ��ٸ� ������, 2 ��ٸ� ����, 3 ����, 4 ������ �Ʒ���, 5 ������ ����, 6 ����
    [SerializeField] Sprite[] itemCursorSprite; //Ŀ�� �ε��� ������ ��������Ʈ �ֱ�

    //���� â ����
    [SerializeField] Tilemap editTilemap; // ����â
    [SerializeField] Tilemap gangTilemap; // ���� Ÿ�ϸ�
    [SerializeField] Tilemap railTilemap; // ���� Ÿ�ϸ�
    [SerializeField] Tilemap ladderTilemap; // ��ٸ� Ÿ�ϸ�
    [SerializeField] Tilemap elevatorPassageTilemap; // ���������� ��� Ÿ�ϸ�
    [SerializeField] Tilemap editBackground; // ���
    public bool isEditOn=false; //����â �����ִ°�

    //��ġ ����
    private int obstacleMask; //��ġ�� ��ֹ� �ִ��� Ȯ��
    private int gangMask; //��ġ�� ���� �ִ��� Ȯ��
    private int groundMask; //��ġ�� �� �ִ��� Ȯ��
    RaycastHit2D hit; //���������� ¦ Ȯ�ν� ���

    // ��Ÿ
    private Dictionary<Vector3Int, GameObject> groundDictionary;
    
    // Ÿ�� �� ��ġ��
    [SerializeField] TileBase gang;
    [SerializeField] TileBase rail;
    [SerializeField] TileBase leftLadder;
    [SerializeField] TileBase rightLadder;
    [SerializeField] TileBase elevatorPassage;
    [SerializeField] GameObject elevatorTop;
    [SerializeField] GameObject elevatorBottom;

    // Ÿ�� ����
    private GameObject objectToErase; //������ ��ü
    private int eraseMask; //������ ���� �ִ��� Ȯ��

    //UI ����
    [SerializeField] GameObject EditOnWindow; //Edit ���� UI
    [SerializeField] GameObject EditInventory; //Edit �κ� UI
    [SerializeField] GameObject ToolBelt; //ToolBelt

    // ����������
    private Vector3 elevatorTopPos;
    private Vector3 elevatorBottomPos;
    private Vector3Int elevatorConstructVec;

    //Edit sound
    public AudioClip[] cursorMoveSound;
    public AudioClip[] installSound;
    public AudioClip[] changeStructSound;
    public AudioClip[] editOnSound;
    public AudioClip[] ErrorSound;

    //��ġ �ٿ�� ����
    [SerializeField] Camera camera; //ī�޶� ��ü
    public float originalCameraSize; //���� ī�޶� ũ��
    [SerializeField] float editCameraSize; //Edit ��ȯ�� ī�޶� ũ�� ��ȯ
    [SerializeField] int heightBound;// ���η� ������(����)
    [SerializeField] int widthBound; // ���η� ������ (����)

    //����
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
                    //����â ������
                    editBackground.gameObject.SetActive(false);
                    cursor.SetActive(false);
                    EditOnWindow.SetActive(false);
                    EditInventory.SetActive(false);
                    ToolBelt.SetActive(true);
                    camera.fieldOfView = originalCameraSize;
                }
                else
                {
                    //����â ���� ��
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
    private void MoveEditCursor() //Ŀ�� �����̱�
    {
        if (!Input.GetKey(KeyCode.LeftAlt) && isEditOn)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) //���� �̵�
            {
                if (editTilemap.WorldToCell(player.transform.position).x - (editTilemap.WorldToCell(cursor.transform.position).x - 1) <= widthBound)
                {
                    SoundFXManager.instance.PlaySoundFXClip(cursorMoveSound, transform, 1.5f);
                    cursor.transform.position += new Vector3(-1, 0, 0);
                }  
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow)) //���� �̵�
            {
                if ((editTilemap.WorldToCell(cursor.transform.position).x + 1) - editTilemap.WorldToCell(player.transform.position).x <= widthBound)
                {
                    SoundFXManager.instance.PlaySoundFXClip(cursorMoveSound, transform, 1.5f);
                    cursor.transform.position += new Vector3(1, 0, 0);
                } 
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow)) //���� �̵�
            {
                if ((editTilemap.WorldToCell(cursor.transform.position).y + 1) - editTilemap.WorldToCell(player.transform.position).y <= heightBound)
                {
                    SoundFXManager.instance.PlaySoundFXClip(cursorMoveSound, transform, 1.5f);
                    cursor.transform.position += new Vector3(0, 1, 0);
                }      
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) //�Ʒ��� �̵�
            {
                if (editTilemap.WorldToCell(player.transform.position).y - (editTilemap.WorldToCell(cursor.transform.position).y - 1) <= heightBound)
                {
                    SoundFXManager.instance.PlaySoundFXClip(cursorMoveSound, transform, 1.5f);
                    cursor.transform.position += new Vector3(0, -1, 0);
                }      
            }
        } 
    }

    private void ChangeItemIndex() //Ŀ�� ��ȯ
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) //Ŀ�� ���� ��������
            {
                itemCursorIndex--;
                SoundFXManager.instance.PlaySoundFXClip(changeStructSound, transform, 1.0f);
                if (itemCursorIndex < 0)
                {
                    itemCursorIndex = itemCursorSprite.Length-1;
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow)) //Ŀ�� ���� ����������
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

    private void InstallBlock() //��� ��ġ
    {
        if (isEditOn && Input.GetKeyDown(KeyCode.F))
        {
            if (CheckCanInstall())
            {
                SoundFXManager.instance.PlaySoundFXClip(installSound, transform, 1.0f);
                switch (itemCursorIndex)
                {
                    case 0: //����
                        GangController.instance.CreateGang(editTilemap.WorldToCell(cursor.transform.position));
                        GM.GangNum--;
                        break;
                    case 1: //������ ��ٸ�
                        ladderTilemap.SetTile(editTilemap.WorldToCell(cursor.transform.position), rightLadder);
                        GM.LadderNum--;
                        break;
                    case 2: //���� ��ٸ�
                        ladderTilemap.SetTile(editTilemap.WorldToCell(cursor.transform.position), leftLadder);
                        GM.LadderNum--;
                        break;
                    case 3://����
                        railTilemap.SetTile(editTilemap.WorldToCell(cursor.transform.position), rail);
                        GM.RailNum--;
                        break;
                    case 4: //���� ����
                        GM.ElevatorDoorNum--;
                        GameObject Top = GameObject.Instantiate(elevatorTop);
                        Top.transform.position = cursor.transform.position;
                        hit = Physics2D.Raycast(Top.transform.position, new Vector2(0, -1), GM.ElevatorPassageNum + 1, obstacleMask);
                        if(hit.collider!=null && hit.collider.gameObject.tag == "Elevator" && !hit.collider.gameObject.GetComponent<Elevator>().isTop) //tag�� �ٲ� �����ϱ�
                        {
                            Top.GetComponent<Elevator>().pair = hit.collider.gameObject;
                            hit.collider.gameObject.GetComponent<Elevator>().pair = Top;
                            elevatorInstall(Top,hit.collider.gameObject);
                            //���� ���� ��ġ �κ� ���� �ٽ� �����غ���
                        }
                        else
                        {
                            saveLoadManager.topDic.Add(Top, null);
                        }
                        
                        break;
                    case 5://���� �Ʒ���
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
                            //���� �ʿ��� ����
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
        // 0 ����, 1 ���� ��ٸ�, 2 �� ��ٸ�, 3 ����, 4 ���������� �Ʒ� ��, 5 ���������� ���� ��
        Collider2D obstacleOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.4f, obstacleMask);
        Collider2D gangOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.4f, gangMask);
        if (itemCursorIndex == 6) //����
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
