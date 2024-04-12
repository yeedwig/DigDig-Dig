using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditController : MonoBehaviour
{
    [SerializeField] GameObject player; 

    //���� Ŀ��
    [SerializeField] GameObject cursor;
    private SpriteRenderer cursorSR;
    private Vector3 cursorPos; //Ŀ�� ��ġ (����)
    private Vector3Int cursorPosInt; // Ŀ�� ��ġ (��ǥ)
    public int itemCursorIndex; //0 ����, 1 ��ٸ� ������, 2 ��ٸ� ����, 3 ����, 4 ������ �Ʒ���, 5 ������ ����, 6 ����
    [SerializeField] Sprite[] itemCursorSprite;
    [SerializeField] float cursorFastMoveStartTimerMax; //������ �����̱� �����ϴ� ��
    [SerializeField] float cursorFastMoveTimerMin; // ������ �����̴� �ּ� ��
    private float cursorFastMoveInterval; //Ŀ�� ������ �����̴� ��
    [SerializeField] float cursorFastMoveTimerSubtract; //Ŀ�� ������ �����̴� �� ���̱�
    private float cursorMoveTimer = 0.0f; 

    //���� â ����
    [SerializeField] Tilemap editTilemap; // ����â
    [SerializeField] Tilemap gangTilemap; // ���� Ÿ�ϸ�
    [SerializeField] Tilemap railTilemap; // ���� Ÿ�ϸ�
    [SerializeField] Tilemap ladderTilemap; // ��ٸ� Ÿ�ϸ�
    [SerializeField] Tilemap elevatorPassageTilemap; // ���������� ��� Ÿ�ϸ�
    [SerializeField] Tilemap editBackground; // ���
    public bool isEditOn=false; //����â �����ִ°�

    //ray,collider
    Collider2D obstacleOnCursor;//Ŀ�� ���� �� �ִ��� Ȯ��
    Collider2D gangOnCursor;//Ŀ�� ���� ���� �ִ��� Ȯ��
    Collider2D eraseOnCursor;//Ŀ�� ���� ���ﲨ �ִ��� Ȯ��
    RaycastHit2D hit;

    // ��Ÿ
    private int obstacleMask;
    private int gangMask;
    private Dictionary<Vector3Int, GameObject> groundDictionary;
    

    // Ÿ�� �� ��ġ��
    [SerializeField] TileBase gang;
    [SerializeField] TileBase rail;
    [SerializeField] TileBase leftLadder;
    [SerializeField] TileBase rightLadder;
    [SerializeField] TileBase elevatorPassage;
    [SerializeField] GameObject elevatorTop;
    [SerializeField] GameObject elevatorBottom;

    // ����������
    private Vector3 elevatorTopPos;
    private Vector3 elevatorBottomPos;
    public float elevatorPassageCount = 10;
    private Vector3Int elevatorConstructVec;

    // Ÿ�� ����
    private GameObject objectToErase;
    private int eraseMask;



    void Start()
    {
        cursorSR = cursor.GetComponent<SpriteRenderer>();
        cursorSR.color = new Color(1, 1, 1, 0.7f);
        obstacleMask = 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Structure");
        eraseMask = 1 << LayerMask.NameToLayer("Structure") | 1 << LayerMask.NameToLayer("ElevatorSub");
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
                    itemCursorIndex = 6;
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                itemCursorIndex++;
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
        if (isEditOn && Input.GetKeyDown(KeyCode.Z))
        {
            if (CheckCanInstall())
            {
                switch (itemCursorIndex)
                {
                    case 0: //����
                        cursorPosInt = editTilemap.WorldToCell(cursor.transform.position);
                        gangTilemap.SetTile(cursorPosInt, gang);
                        groundDictionary[cursorPosInt].GetComponent<Ground>().gangInstalled = true;
                        break;
                    case 1: //������ ��ٸ�
                        Debug.Log("Test");
                        ladderTilemap.SetTile(editTilemap.WorldToCell(cursor.transform.position), rightLadder);
                        break;
                    case 2: //���� ��ٸ�
                        ladderTilemap.SetTile(editTilemap.WorldToCell(cursor.transform.position), leftLadder);
                        break;
                    case 3://����
                        railTilemap.SetTile(editTilemap.WorldToCell(cursor.transform.position), rail);
                        break;
                    case 4: //���� ����
                        GameObject Top = GameObject.Instantiate(elevatorTop);
                        Top.transform.position = cursor.transform.position;
                        hit = Physics2D.Raycast(Top.transform.position, new Vector2(0, -1), elevatorPassageCount, obstacleMask);
                        if(hit.collider!=null && hit.collider.gameObject.tag == "Elevator" && !hit.collider.gameObject.GetComponent<Elevator>().isTop) //tag�� �ٲ� �����ϱ�
                        {
                            Top.GetComponent<Elevator>().pair = hit.collider.gameObject;
                            hit.collider.gameObject.GetComponent<Elevator>().pair = Top;
                            elevatorInstall(Top,hit.collider.gameObject);
                        }
                        break;
                    case 5://���� �Ʒ���
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
                    case 6:
                        Debug.Log(objectToErase.name);
                        if (objectToErase.name == "GangTilemap")
                        {
                            cursorPosInt = editTilemap.WorldToCell(cursor.transform.position);
                            gangTilemap.SetTile(cursorPosInt, null);
                            groundDictionary[cursorPosInt].GetComponent<Ground>().gangInstalled = false;
                        }
                        else if(objectToErase.name == "ElevatorPassageTilemap")
                        {
                            Debug.Log("test");
                            hit = Physics2D.Raycast(cursor.transform.position, new Vector2(0, 1), 100000.0f, obstacleMask);
                            DestroyElevatorPassage(hit.collider.gameObject, hit.collider.gameObject.GetComponent<Elevator>().pair);
                        }
                        else if (objectToErase.tag == "Elevator")
                        {
                            if (objectToErase.GetComponent<Elevator>().pair == null)
                            {
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
        }
    }
    private bool CheckCanInstall()
    {
        // 0 ����, 1 ���� ��ٸ�, 2 �� ��ٸ�, 3 ����, 4 ���������� �Ʒ� ��, 5 ���������� ���� ��
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
                    return true;
                }
                if (gangOnCursor == null)
                {
                    return false;
                }
                else
                {
                    if (itemCursorIndex == 4 || itemCursorIndex == 5)
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
            gangTilemap.SetTile(elevatorConstructVec, gang);
            groundDictionary[elevatorConstructVec].GetComponent<Ground>().gangInstalled = true;
            elevatorBottomPos.y += 1f;
        }
    }

    private void DestroyElevatorPassage(GameObject d1,GameObject d2)
    {
        if (d1.transform.position.y > d2.transform.position.y)
        {
            elevatorTopPos = d1.transform.position;
            elevatorBottomPos = d2.transform.position + new Vector3(0, 1f, 0);
        }
        else
        {
            elevatorTopPos = d2.transform.position;
            elevatorBottomPos = d1.transform.position + new Vector3(0, 1f, 0);
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
