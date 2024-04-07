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
    RaycastHit2D hit;

    // ��Ÿ
    private int obstacleMask;
    private int gangMask;
    private Dictionary<Vector3Int, GameObject> groundDictionary;
    

    // Ÿ��
    [SerializeField] TileBase gang;
    [SerializeField] TileBase rail;
    [SerializeField] TileBase leftLadder;
    [SerializeField] TileBase rightLadder;

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
                        cursorPosInt = editTilemap.WorldToCell(cursor.transform.position);
                        gangTilemap.SetTile(cursorPosInt, gang);
                        groundDictionary[cursorPosInt].GetComponent<Ground>().gangInstalled = true;
                        break;
                    case 1: //������ ��ٸ�
                        ladderTilemap.SetTile(editBackground.WorldToCell(cursor.transform.position), rightLadder);
                        break;
                    case 2: //���� ��ٸ�
                        ladderTilemap.SetTile(editBackground.WorldToCell(cursor.transform.position), leftLadder);
                        break;
                    case 3://����
                        railTilemap.SetTile(editBackground.WorldToCell(cursor.transform.position), rail);
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

    private void InstallElevatorPassage()
    {
        
        if (elevatorStartPosition.y > elevatorEndPosition.y)
        {
            Vector3Int startPos = editBackground.WorldToCell(elevatorStartPosition - new Vector3(0, 1, 0));
            Vector3Int endPos = editBackground.WorldToCell(elevatorEndPosition + new Vector3(0, 1, 0));
            while (startPos.y > endPos.y)
            {
                //GameObject elevatorPassagePrefab = Instantiate(itemPrefabs[6]);
                //elevatorPassagePrefab.transform.position = startPos + new Vector3(0.5f, 0.5f, 0);
                startPos.y--;
            }
        }
        else
        {
            Vector3Int startPos = editBackground.WorldToCell(elevatorStartPosition);
            Vector3Int endPos = editBackground.WorldToCell(elevatorEndPosition + new Vector3(0, 1, 0));
            while (startPos.y < endPos.y)
            {
                //GameObject elevatorPassagePrefab = Instantiate(itemPrefabs[6]);
                //elevatorPassagePrefab.transform.position = startPos + new Vector3(0.5f, 0.5f, 0);
                startPos.y++;
            }
        }
        //ground ���� �߰��ϱ�
        editBackground.ClearAllTiles();

    }
}
