using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditController : MonoBehaviour
{
    private GameObject player;

    [SerializeField] GameObject cursor;
    private SpriteRenderer cursorSR;
    private int itemCursorIndex; //0 ����, 1 ��ٸ�, 2 ����, 3 ������, 4 ���� ���
    [SerializeField] Sprite[] itemCursorSprite; 
    

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
        // ���� ���� ���� ��ġ�� ���¶�� ��� sprite�� ����, �ű⼭�� �¿�� ���� �Ұ���

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
                        Vector3Int cursorPos = editBackground.WorldToCell(cursor.transform.position);
                        Ground ground = groundDictionary[cursorPos].GetComponent<Ground>();
                        ground.gangInstalled = true;
                        ground.ChangeSpriteByCurrentHealth();
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
        switch (itemCursorIndex)
        {
            case 0: //����
                var ground = Physics2D.OverlapCircle(cursor.transform.position, 0.2f, layerMask);
                if (ground == null)
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
    }
}
