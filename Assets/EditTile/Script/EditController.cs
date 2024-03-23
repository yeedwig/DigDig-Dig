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

    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        selectTilemap = GameObject.Find("Edit").GetComponent<Tilemap>();
        editBackground = GameObject.Find("EditBackground").GetComponent<Tilemap>();
        cursorSR = cursor.GetComponent<SpriteRenderer>();
        cursor.SetActive(false);
        isEditOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckEdit();
        MoveEditCursor();
        ChangeItemIndex();
        if(isEditOn)
        {
            cursor.transform.position = editPos;
        }
        bool test = CheckCanInstall();
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
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                editPos.x++;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                editPos.y++;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                editPos.y--;
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
    private bool CheckCanInstall()
    {
        bool canInstall = false;
        switch (itemCursorIndex)
        {
            case 0:
                
                break;
            default:
                break;
        }
        return canInstall;
    }
}
