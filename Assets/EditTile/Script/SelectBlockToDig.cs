using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SelectBlockToDig : MonoBehaviour
{
    private GameObject player;
    [SerializeField] TileBase selectTile;
    private Tilemap selectTilemap;
    private Vector2 direction;
    private int layerMask;
    private Vector3Int gridPlayerPosition;
    private ToolManager toolManager;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        selectTilemap = GameObject.Find("SelectGroundToDig").GetComponent<Tilemap>();
        toolManager = GameObject.Find("ToolManager").GetComponent<ToolManager>();
        layerMask = 1 << LayerMask.NameToLayer("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        selectTilemap.ClearAllTiles();
        if (Input.GetKey(KeyCode.LeftArrow) && toolManager.curToolType==0)
        {
            direction = new Vector2(-1, 0);
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position, direction, 1.0f, layerMask);

            if (hit.collider != null)
            {
                gridPlayerPosition = selectTilemap.WorldToCell(player.transform.position);
                selectTilemap.SetTile(gridPlayerPosition + new Vector3Int(-1, 0, 0), selectTile);
            }

        }
        else if (Input.GetKey(KeyCode.RightArrow) && toolManager.curToolType == 0)
        {
            direction = new Vector2(1, 0);
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position, direction, 1.0f, layerMask);

            if (hit.collider != null)
            {
                gridPlayerPosition = selectTilemap.WorldToCell(player.transform.position);
                selectTilemap.SetTile(gridPlayerPosition + new Vector3Int(1, 0, 0), selectTile);
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow) && toolManager.curToolType <= 1)
        {
            direction = new Vector2(0, -1);
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position, direction, 1.0f, layerMask);

            if (hit.collider != null)
            {
                gridPlayerPosition = selectTilemap.WorldToCell(player.transform.position);
                selectTilemap.SetTile(gridPlayerPosition + new Vector3Int(0, -1, 0), selectTile);
            }
        }
    }
}
