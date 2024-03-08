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
    public int[,] digRuin1= new int[7, 5]
    {
        {0,1,1,1,1},
        {0,1,1,1,1},
        {1,1,1,1 , 1},
        {1,1,1,1,1 },
        {1,1,1,1,1 },
        {1,1,1,1 ,1},
        {1,1,1,1 ,1}
    };

    void Awake()
    {
       groundDictionary =new Dictionary<Vector3Int, GameObject>();
     

   
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            
            int x, y,width,length;
            x = Random.Range(0, 8);
            y = Random.Range(0, -3);
            length = digRuin1.GetLength(0);
            width = digRuin1.GetLength(1);
            for(int i=0; i<length; i++)
            {
                for(int j=0; j<width; j++)
                {
                    if (digRuin1[i,j] == 1)
                    {
                        Destroy(groundDictionary[new Vector3Int(x+j,y-i, 0)]);
                    }
                }
            }
        
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
