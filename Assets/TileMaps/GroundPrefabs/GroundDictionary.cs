using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundDictionary : MonoBehaviour
{
    public Dictionary<Vector3Int, GameObject> groundDictionary;
    public GameObject ruins;

    //기준점
    public GameObject point;

    public Tilemap groundTileMap;

    void Awake()
    {
       groundDictionary =new Dictionary<Vector3Int, GameObject>();       
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            
            int x, y;
            x = Random.Range(-12, 3);
            y = Random.Range(-8, -3);
            
            for(int i = x - 1; i <= x + 1; i++)
            {
                for(int j = y - 1;j<= y + 1; j++)
                {
                    Destroy(groundDictionary[new Vector3Int(i,j,0)]);
                }
            }
            GameObject test=Instantiate(ruins);
            test.transform.position = groundTileMap.CellToWorld(new Vector3Int(x, y, 0));
            
        }
    }

    public void AddToGroundDictionary(Vector3Int gridPosition, GameObject groundObject)
    {
        
        groundDictionary.Add(gridPosition, groundObject);
    }
}
