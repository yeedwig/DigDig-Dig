using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditController : MonoBehaviour
{
    private GameObject player;

    [SerializeField] GameObject cursor;
    private SpriteRenderer cursorSR;
    private int itemCursorIndex; //0 갱도, 1 사다리, 2 레일, 3 엘베문, 4 엘베 통로
    [SerializeField] Sprite[] itemCursorSprite;
    [SerializeField] GameObject[] itemPrefabs;


    private Tilemap selectTilemap;
    private Tilemap editBackground;
    public bool isEditOn;
    private Vector3 editPos;

    private int layerMask;

    private Dictionary<Vector3Int, GameObject> groundDictionary;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        selectTilemap = GameObject.Find("Edit").GetComponent<Tilemap>();
        editBackground = GameObject.Find("EditBackground").GetComponent<Tilemap>();
        cursorSR = cursor.GetComponent<SpriteRenderer>();
        layerMask = 1 << LayerMask.NameToLayer("Ground");
        groundDictionary = GameObject.Find("GroundDictionary").GetComponent<GroundDictionary>().groundDictionary;
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
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                editPos.x--;
                cursor.transform.position = editPos;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                editPos.x++;
                cursor.transform.position = editPos;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                editPos.y++;
                cursor.transform.position = editPos;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                editPos.y--;
                cursor.transform.position = editPos;
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
                    itemCursorIndex = 3;
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                itemCursorIndex++;
                if (itemCursorIndex > 3)
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
                switch (itemCursorIndex)
                {
                    case 0: //갱도
                        cursorPos = editBackground.WorldToCell(cursor.transform.position);
                        Ground ground = groundDictionary[cursorPos].GetComponent<Ground>();
                        ground.gangInstalled = true;
                        ground.ChangeSpriteByCurrentHealth();
                        break;
                    case 2://레일
                        cursorPos = editBackground.WorldToCell(cursor.transform.position);
                        GameObject rail = Instantiate(itemPrefabs[2]);
                        rail.transform.position = cursorPos+new Vector3(0.5f,0.5f,0);
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
        switch (itemCursorIndex)
        {
            case 0: //갱도
                groundOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.2f, layerMask);
                if (groundOnCursor == null)
                {
                    canInstall = true;
                }
                break;
            case 2: //레일
                groundOnCursor = Physics2D.OverlapCircle(cursor.transform.position, 0.2f, layerMask);
                hit = Physics2D.Raycast(cursor.transform.position, new Vector2(0,-1), 0.7f, layerMask);
                if (groundOnCursor == null && hit.collider!=null)
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
