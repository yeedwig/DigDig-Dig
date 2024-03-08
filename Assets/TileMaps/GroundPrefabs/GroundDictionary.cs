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
    public int[,] digRuin1= new int[7, 4]
    {
        {0,1,1,1},
        {0,1,1,1},
        {1,1,1,1 },
        {1,1,1,1 },
        {1,1,1,1 },
        {1,1,1,1 },
        {1,1,1,1 }
    };

    void Awake()
    {
       groundDictionary =new Dictionary<Vector3Int, GameObject>();
     

   
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            
            int x, y;
            x = Random.Range(0, 1);
            y = Random.Range(0, -1);
            
            /*
            for(int i = x; i <= x + 2; i++)
            {
                for(int j = y;j>=-2; j--)
                {
                    Destroy(groundDictionary[new Vector3Int(i,j,0)]);
                }
            }*/

            GameObject test=Instantiate(ruins);
            test.transform.position = groundTileMap.CellToWorld(new Vector3Int(x, y, 0));
            
        }
    }

    public void AddToGroundDictionary(Vector3Int gridPosition, GameObject groundObject)
    {
        
        groundDictionary.Add(gridPosition, groundObject);
    }
}
