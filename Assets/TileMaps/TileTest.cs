using System.Collections;
using System.Collections.Generic;
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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Vector3Int gridPosition = tilemap.WorldToCell(player.transform.position);
            Debug.Log("Space");
            Debug.Log(gridPosition.x);
            Debug.Log(gridPosition.y);
            Debug.Log(gridPosition.z);
            tilemap.SetTile(gridPosition-new Vector3Int(0,1,0), tile2);
            
        }
    }
}
