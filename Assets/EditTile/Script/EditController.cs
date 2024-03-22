using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditController : MonoBehaviour
{
    private GameObject player;
    [SerializeField] TileBase selectTile;
    [SerializeField] TileBase editTile;

    [SerializeField] GameObject cursor;
    private SpriteRenderer cursorSR;
    private int itemCursorIndex; //0 갱도, 1 사다리, 2 레일, 3 엘베문, 4 엘베 통로
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
        CreateEditBackground();
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
            
            //selectTilemap.ClearAllTiles();
            //selectTilemap.SetTile(editPos, selectTile);
        }
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
                editBackground.gameObject.SetActive(true);
                editPos = selectTilemap.WorldToCell(player.transform.position);
                editPos.x += 0.5f;
                editPos.y += 0.5f;
                cursorSR.sprite = itemCursorSprite[itemCursorIndex];
            }
            isEditOn = !isEditOn;
        }
    }
    private void CreateEditBackground()
    {
        int mapWidth = GameObject.Find("MapGenerator").GetComponent<MapGenerator>().mapWidth;
        int mapHeight = GameObject.Find("MapGenerator").GetComponent<MapGenerator>().mapHeight;
        for(int i=0;i<mapHeight; i++)
        {
            for(int j=0;j<mapWidth; j++)
            {
                editBackground.SetTile(new Vector3Int(j, -i, 0), editTile);
            }
        }
        editBackground.gameObject.SetActive(false);
        
    }

    private void MoveEditCursor()
    {
        if (!Input.GetKey(KeyCode.C))
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
        if (Input.GetKey(KeyCode.C))
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
}
