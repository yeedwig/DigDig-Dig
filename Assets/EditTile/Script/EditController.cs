using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditController : MonoBehaviour
{
    private GameObject player;
    [SerializeField] TileBase selectTile;
    [SerializeField] TileBase editTile;
    private Tilemap selectTilemap;
    private Tilemap editBackground;
    public bool isEditOn;
    private Vector3Int editPos;

    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        selectTilemap = GameObject.Find("Edit").GetComponent<Tilemap>();
        editBackground = GameObject.Find("EditBackground").GetComponent<Tilemap>();
        CreateEditBackground();
        isEditOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckEdit();
        if(isEditOn)
        {
            
            
        }
        else
        {
            
        }
        
    }

    void CheckEdit()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(isEditOn)
            {
                editBackground.gameObject.SetActive(false);
                selectTilemap.ClearAllTiles();
            }
            else
            {
                editBackground.gameObject.SetActive(true);
                Vector3Int gridPlayerPosition = selectTilemap.WorldToCell(player.transform.position);
                selectTilemap.SetTile(gridPlayerPosition + new Vector3Int(0, -1, 0), selectTile);
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
}
