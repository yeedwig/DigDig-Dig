using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Select : MonoBehaviour
{
    public GameObject player;
    public TileBase select;
    public Tilemap selectTilemap;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.LeftArrow)) 
        {
            selectTilemap.ClearAllTiles();
            direction = new Vector2(-1, 0);
            int layerMask = 1 << LayerMask.NameToLayer("Ground");
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position, direction, 1.0f, layerMask);

            if (hit.collider != null)
            {
                Vector3Int gridPlayerPosition = selectTilemap.WorldToCell(player.transform.position);
                selectTilemap.SetTile(gridPlayerPosition + new Vector3Int(-1, 0, 0), select);
            }
            
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            selectTilemap.ClearAllTiles();
            direction = new Vector2(1, 0);
            int layerMask = 1 << LayerMask.NameToLayer("Ground");
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position, direction, 1.0f, layerMask);

            if (hit.collider != null)
            {
                Vector3Int gridPlayerPosition = selectTilemap.WorldToCell(player.transform.position);
                selectTilemap.SetTile(gridPlayerPosition + new Vector3Int(1, 0, 0), select);
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            selectTilemap.ClearAllTiles();
            direction = new Vector2(0,-1);
            int layerMask = 1 << LayerMask.NameToLayer("Ground");
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position, direction, 1.0f, layerMask);

            if (hit.collider != null)
            {
                Vector3Int gridPlayerPosition = selectTilemap.WorldToCell(player.transform.position);
                selectTilemap.SetTile(gridPlayerPosition + new Vector3Int(0, -1, 0), select);
            }
        }
        else
        {
            selectTilemap.ClearAllTiles();
        }
        
    }
}
