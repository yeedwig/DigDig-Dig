using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditController : MonoBehaviour
{
    private GameObject player;

    [SerializeField] GameObject cursor;
    private SpriteRenderer cursorSR;
    private int itemCursorIndex; //0 갱도, 1 사다리 오른쪽, 2 사다리 왼쪽, 3 레일, 4 엘베문, 5 엘베 통로
    [SerializeField] Sprite[] itemCursorSprite;
    [SerializeField] GameObject[] itemPrefabs;


    private Tilemap selectTilemap;
    private Tilemap editBackground;
    public bool isEditOn;
    private Vector3 editPos;

    private int layerMask;

    private Dictionary<Vector3Int, GameObject> groundDictionary;

    [SerializeField] float cursorMoveTimerMax;
    private float cursorMoveTimer;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        selectTilemap = GameObject.Find("Edit").GetComponent<Tilemap>();
        editBackground = GameObject.Find("EditBackground").GetComponent<Tilemap>();
        cursorSR = cursor.GetComponent<SpriteRenderer>();
        layerMask = 1 << LayerMask.NameToLayer("Ground");
        groundDictionary = GameObject.Find("GroundDictionary").GetComponent<GroundDictionary>().groundDictionary;
        cursorMoveTimer = 0.0f;
        cursor.SetActive(false);
        isEditOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckEdit();
        MoveEditCursor();
        ChangeItemIndex();
        InstallBlock();
        
        
    }

    void CheckEdit()
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
                cursor.SetActive(true);
                itemCursorIndex = 0;
                editPos = selectTilemap.WorldToCell(player.transform.position);
                editPos.x += 0.5f;
                editPos.y += 0.5f;
                cursor.transform.position = editPos;
                cursorSR.sprite = itemCursorSprite[itemCursorIndex];
                cursorSR.color = new Color(1, 1, 1, 0.7f);
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
                if(cursorMoveTimer > 0)
                {
                    cursorMoveTimer--;
                }
                else
                {
                    editPos.x--;
                    cursor.transform.position = editPos;
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
                    editPos.x++;
                    cursor.transform.position = editPos;
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
                    editPos.y++;
                    cursor.transform.position = editPos;
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
                    editPos.y--;
                    cursor.transform.position = editPos;
                    cursorMoveTimer = cursorMoveTimerMax;
                }
            }
            else
            {
                cursorMoveTimer = 20.0f;
            }
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
                    itemCursorIndex = 4;
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                itemCursorIndex++;
                if (itemCursorIndex > 4)
                {
                    itemCursorIndex = 0;
                }
            }
            cursorSR.sprite = itemCursorSprite[itemCursorIndex];
        }
        // 만약 엘베 문을 설치한 상태라면 통로 sprite로 변경, 거기서는 좌우로 변경 불가능

    }

    private void InstallBlock()
    {
        if (isEditOn && Input.GetKeyDown(KeyCode.Z))
        {
            if (CheckCanInstall())
            {
                Vector3Int cursorPos;
                Ground ground;
                switch (itemCursorIndex)
                {
                    case 0: //갱도
                        cursorPos = editBackground.WorldToCell(cursor.transform.position);
                        ground = groundDictionary[cursorPos].GetComponent<Ground>();
                        ground.gangInstalled = true;
                        ground.ChangeSpriteByCurrentHealth();
                        break;
                    case 1: //오른쪽 사다리
                        cursorPos = editBackground.WorldToCell(cursor.transform.position);
                        ground = groundDictionary[cursorPos].GetComponent<Ground>();
                        GameObject ladderRight = Instantiate(itemPrefabs[itemCursorIndex]);
                        ladderRight.transform.position = cursorPos + new Vector3(0.5f, 0.5f, 0);
                        ground.structureInstalled = true;
                        break;
                    case 2: //왼쪽 사다리
                        cursorPos = editBackground.WorldToCell(cursor.transform.position);
                        ground = groundDictionary[cursorPos].GetComponent<Ground>();
                        GameObject ladderLeft = Instantiate(itemPrefabs[itemCursorIndex]);
                        ladderLeft.transform.position = cursorPos + new Vector3(0.5f, 0.5f, 0);
                        ground.structureInstalled = true;
                        break;
                    case 3://레일
                        cursorPos = editBackground.WorldToCell(cursor.transform.position);
                        ground = groundDictionary[cursorPos].GetComponent<Ground>();
                        GameObject rail = Instantiate(itemPrefabs[itemCursorIndex]);
                        rail.transform.position = cursorPos+new Vector3(0.5f,0.5f,0);
                        ground.structureInstalled = true;
                        break;
                    default:
                        break;
                }
            }
        }
    }
    private bool CheckCanInstall()
    {
        bool canInstall = false;
        Collider2D groundOnCursor;
        RaycastHit2D hit;
        Vector3Int cursorPos;
        Ground ground;
        switch (itemCursorIndex)
        {
            case 0: //갱도
                groundOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.2f, layerMask);
                if (groundOnCursor == null)
                {
                    canInstall = true;
                }
                break;
            case 1: //오른쪽 사다리
                groundOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.2f, layerMask);
                hit = Physics2D.Raycast(cursor.transform.position, new Vector2(1, 0), 0.7f, layerMask);
                cursorPos = editBackground.WorldToCell(cursor.transform.position);
                ground = groundDictionary[cursorPos].GetComponent<Ground>();
                if (groundOnCursor == null && hit.collider != null && ground.gangInstalled && !ground.structureInstalled)
                {
                    canInstall = true;
                }
                break;
            case 2://왼쪽 사다리
                groundOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.2f, layerMask);
                hit = Physics2D.Raycast(cursor.transform.position, new Vector2(-1, 0), 0.7f, layerMask);
                cursorPos = editBackground.WorldToCell(cursor.transform.position);
                ground = groundDictionary[cursorPos].GetComponent<Ground>();
                if (groundOnCursor == null && hit.collider != null && ground.gangInstalled && !ground.structureInstalled)
                {
                    canInstall = true;
                }
                break;
            case 3: //레일
                groundOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.2f, layerMask);
                hit = Physics2D.Raycast(cursor.transform.position, new Vector2(0,-1), 0.7f, layerMask);
                cursorPos = editBackground.WorldToCell(cursor.transform.position);
                ground = groundDictionary[cursorPos].GetComponent<Ground>();
                if (groundOnCursor == null && hit.collider!=null && ground.gangInstalled && !ground.structureInstalled)
                {
                    canInstall = true;
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
        Gizmos.DrawWireSphere(cursor.transform.position, 0.2f);
        Gizmos.DrawLine(cursor.transform.position,cursor.transform.position+new Vector3(0,-1,0));
    }
}
