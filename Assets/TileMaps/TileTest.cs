using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileTest : MonoBehaviour
{
    public GameObject player;
    public Tilemap tilemap;
    public TileBase tile1,tile2;
    private Dictionary<Vector3Int,float> tiles = new Dictionary<Vector3Int,float>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, new Vector2(0, -1) * 3.0f, Color.red);
        if (Input.GetKey(KeyCode.E))
        {
            Vector2 direction = new Vector2(0,0);
            if(Input.GetKey(KeyCode.LeftArrow))
            {
                direction = new Vector2(-1, 0);
            }
            else if(Input.GetKey(KeyCode.RightArrow))
            {
                direction = new Vector2(1, 0);
            }
            else if( Input.GetKey(KeyCode.DownArrow))
            {
                direction = new Vector2(0, -1);
            }
            
            int layerMask = 1 << LayerMask.NameToLayer("Ground");
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position,direction, 3.0f,layerMask);
            
            if (hit.collider != null)
            {
                hit.transform.gameObject.GetComponent<Ground>().takeDamage(0.3f);
            }
            
        }
        if(Input.GetKey(KeyCode.Space))
        {
            Debug.Log("clicked");
            Vector3Int gridPosition = tilemap.WorldToCell(player.transform.position);
            Debug.Log(gridPosition.x);
            Debug.Log(gridPosition.y);
            
            tilemap.SwapTile(tile1, tile2);
        }
    }
}
