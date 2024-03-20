using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditController : MonoBehaviour
{
    private GameObject player;
    [SerializeField] TileBase selectTile;
    private Tilemap selectTilemap;
    private bool isEditOn;
    private Vector3Int editPos;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        selectTilemap = GameObject.Find("Edit").GetComponent<Tilemap>();
        isEditOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckEdit();
        if(isEditOn)
        {
            Vector3Int gridPlayerPosition = selectTilemap.WorldToCell(player.transform.position);
            selectTilemap.SetTile(gridPlayerPosition + new Vector3Int(1, 0, 0), selectTile);
        }
        else
        {
            selectTilemap.ClearAllTiles();
        }
        
    }

    void CheckEdit()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isEditOn = !isEditOn;
        }
    }
}
