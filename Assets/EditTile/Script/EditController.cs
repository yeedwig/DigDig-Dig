using System.Collections;
using System.Collections.Generic;
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
    private int itemCursorIndex; //0 ����, 1 ��ٸ� ������, 2 ��ٸ� ����, 3 ����, 4 ������ �Ʒ���, 5 ������ ����
    [SerializeField] Sprite[] itemCursorSprite;
    [SerializeField] GameObject[] itemPrefabs;
    [SerializeField] float cursorFastMoveStartTimerMax; //������ �����̱� �����ϴ� ��
    [SerializeField] float cursorFastMoveTimerMin; // ������ �����̴� �ּ� ��
    private float cursorFastMoveInterval; //Ŀ�� ������ �����̴� ��
    [SerializeField] float cursorFastMoveTimerSubtract; //Ŀ�� ������ �����̴� �� ���̱�
    private float cursorMoveTimer = 0.0f; 

    //���� â ����
    [SerializeField] Tilemap editTilemap; // ����â
    [SerializeField] Tilemap gangTilemap; // ���� Ÿ�ϸ�
    [SerializeField] Tilemap editBackground; // ���
    public bool isEditOn=false; //����â �����ִ°�
    private bool canInstall; //��ġ �����Ѱ�

    //����������
    private bool startInstallingElevator=false; //���������� ��ġ ����
    [SerializeField] TileBase elevatorPassage; //���������� ���
    private Vector3 elevatorStartPosition;
    private Vector3 elevatorEndPosition;

    //ray,collider
    Collider2D obstacleOnCursor;//Ŀ�� ���� �� �ִ��� Ȯ��
    Collider2D gangOnCursor;//Ŀ�� ���� ���� �ִ��� Ȯ��
    RaycastHit2D hit; //Ŀ�� ������
    RaycastHit2D rightHit; //Ŀ�� ������
    RaycastHit2D leftHit; //Ŀ�� ����
    private RaycastHit2D leftDiagonal;
    private RaycastHit2D under;
    private RaycastHit2D rightDiagonal;
    private RaycastHit2D up;

    // ��Ÿ
    private int obstacleMask;
    private int gangMask;
    private Dictionary<Vector3Int, GameObject> groundDictionary;
    private Ground ground; //��
    [SerializeField] TileBase gang;

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
                startInstallingElevator = false;
                cursor.SetActive(true);
                editBackground.gameObject.SetActive(true);
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
                    case 0: //����

                        /*
                        cursorPosInt = editBackground.WorldToCell(cursor.transform.position);
                        ground = groundDictionary[cursorPosInt].GetComponent<Ground>();
                        ground.gangInstalled = true;
                        ground.ChangeSpriteByCurrentHealth();
                        */
                        cursorPosInt = editTilemap.WorldToCell(cursor.transform.position);
                        gangTilemap.SetTile(cursorPosInt, gang);
                        break;
                    case 1: //������ ��ٸ�
                        cursorPosInt = editBackground.WorldToCell(cursor.transform.position);
                        ground = groundDictionary[cursorPosInt].GetComponent<Ground>();
                        GameObject ladderRight = Instantiate(itemPrefabs[itemCursorIndex]);
                        ladderRight.transform.position = cursorPosInt + new Vector3(0.5f, 0.5f, 0);
                        ground.structureInstalled = true;
                        break;
                    case 2: //���� ��ٸ�
                        cursorPosInt = editBackground.WorldToCell(cursor.transform.position);
                        ground = groundDictionary[cursorPosInt].GetComponent<Ground>();
                        GameObject ladderLeft = Instantiate(itemPrefabs[itemCursorIndex]);
                        ladderLeft.transform.position = cursorPosInt + new Vector3(0.5f, 0.5f, 0);
                        ground.structureInstalled = true;
                        break;
                    case 3://����
                        cursorPosInt = editBackground.WorldToCell(cursor.transform.position);
                        ground = groundDictionary[cursorPosInt].GetComponent<Ground>();
                        GameObject rail = Instantiate(itemPrefabs[itemCursorIndex]);
                        rail.transform.position = cursorPosInt+new Vector3(0.5f,0.5f,0);
                        ground.structureInstalled = true;
                        break;
                    case 4: //���������� �� �Ʒ���
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

        // 0 ����, 1 ���� ��ٸ�, 2 �� ��ٸ�, 3 ����, 4 ���������� �Ʒ� ��, 5 ���������� ���� ��
        // ���� ��ġ�� Ÿ�Ϸ� �ٲܱ�?
        obstacleOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.4f, obstacleMask);
        gangOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.4f, gangMask);
        if (obstacleOnCursor != null)
        {
            Debug.Log("Found obstacle");
            return false;
        }
        else
        {
            if(itemCursorIndex == 0)
            {
                Debug.Log("Not found");
                return true;
            }
            if (gangOnCursor == null)
            {
                return false;
            }
            else
            {
                if (itemCursorIndex == 1)
                {
                    rightHit = Physics2D.Raycast(cursor.transform.position, new Vector2(1, 0), 0.7f, obstacleMask);
                    if (rightHit.collider != null) return true;
                }
                else if (itemCursorIndex == 2)
                {
                    leftHit = Physics2D.Raycast(cursor.transform.position, new Vector2(-1, 0), 0.7f, obstacleMask);
                    if(leftHit.collider != null) return true;
                }
                else if (itemCursorIndex == 3)
                {

                }
            }

        }


        switch (itemCursorIndex)
        {
            case 0: //����
                obstacleOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.49f, obstacleMask);
                
                if (obstacleOnCursor == null)
                {
                    canInstall = true;
                }
                break;
            case 1: //������ ��ٸ�
                obstacleOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.2f, obstacleMask);
                hit = Physics2D.Raycast(cursor.transform.position, new Vector2(1, 0), 0.7f, obstacleMask);
                cursorPosInt = editTilemap.WorldToCell(cursor.transform.position);
                ground = groundDictionary[cursorPosInt].GetComponent<Ground>();
                if (obstacleOnCursor == null && hit.collider != null && ground.gangInstalled && !ground.structureInstalled)
                {
                    canInstall = true;
                }
                break;
            case 2://���� ��ٸ�
                obstacleOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.2f, obstacleMask);
                hit = Physics2D.Raycast(cursor.transform.position, new Vector2(-1, 0), 0.7f, obstacleMask);
                cursorPosInt = editBackground.WorldToCell(cursor.transform.position);
                ground = groundDictionary[cursorPosInt].GetComponent<Ground>();
                if (obstacleOnCursor == null && hit.collider != null && ground.gangInstalled && !ground.structureInstalled)
                {
                    canInstall = true;
                }
                break;
            case 3: //����
                obstacleOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.2f, obstacleMask);
                hit = Physics2D.Raycast(cursor.transform.position, new Vector2(0,-1), 0.7f, obstacleMask);
                cursorPosInt = editBackground.WorldToCell(cursor.transform.position);
                ground = groundDictionary[cursorPosInt].GetComponent<Ground>();
                if (obstacleOnCursor == null && hit.collider!=null && ground.gangInstalled && !ground.structureInstalled)
                {
                    canInstall = true;
                }
                break;
            case 4: //���������� �� �Ʒ���
                obstacleOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.2f, obstacleMask);
                leftDiagonal=Physics2D.Raycast(cursor.transform.position, new Vector2(-0.9f, -1), 1.0f, obstacleMask); 
                under=Physics2D.Raycast(cursor.transform.position, new Vector2(0, -1), 0.7f, obstacleMask); 
                rightDiagonal=Physics2D.Raycast(cursor.transform.position, new Vector2(0.9f, -1), 1.0f, obstacleMask);
                cursorPosInt = editBackground.WorldToCell(cursor.transform.position);
                ground = groundDictionary[cursorPosInt].GetComponent<Ground>();
                if (obstacleOnCursor == null&&leftDiagonal.collider != null&&rightDiagonal.collider != null && ground.gangInstalled && !ground.structureInstalled)
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
                obstacleOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.2f, obstacleMask);
                leftDiagonal = Physics2D.Raycast(cursor.transform.position, new Vector2(-0.9f, -1), 1.0f, obstacleMask);
                up = Physics2D.Raycast(cursor.transform.position, new Vector2(0, 1), 0.7f, obstacleMask);
                rightDiagonal = Physics2D.Raycast(cursor.transform.position, new Vector2(0.9f, -1), 1.0f, obstacleMask);
                cursorPosInt = editBackground.WorldToCell(cursor.transform.position);
                ground = groundDictionary[cursorPosInt].GetComponent<Ground>();
                if(obstacleOnCursor ==null && leftDiagonal.collider != null && rightDiagonal.collider != null && ground.gangInstalled && !ground.structureInstalled)
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
        //ground ���� �߰��ϱ�
        editBackground.ClearAllTiles();

    }
}
