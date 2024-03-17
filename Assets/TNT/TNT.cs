using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using Unity.Mathematics;

public class TNT : MonoBehaviour
{
    [SerializeField] int TNTTimer;
    [SerializeField] int ToolID;
    private float bigTNTDamage;
    private float smallTNTDamage;
    public Tilemap groundTileMap;
    public Dictionary<Vector3Int, GameObject> groundDictionary;
    //폭발 범위 무조건 크기 홀수
    public int[,] bigTNTRange = new int[9, 9]
    {
        {0,0,0,1,1,1,0,0,0},
        {0,0,1,1,1,1,1,0,0},
        {0,1,1,1,1,1,1,1,0},
        {1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,1,1,1,1},
        {0,1,1,1,1,1,1,1,0},
        {0,0,1,1,1,1,1,0,0},
        {0,0,0,1,1,1,0,0,0}
    };
    public int[,] smallTNTRange = new int[5, 5]
    {
        {0,0,1,0,0},
        {0,1,1,1,0},
        {1,1,1,1,1},
        {0,1,1,1,0},
        {0,0,1,0,0}
    };

    // Start is called before the first frame update
    void Start()
    {
        groundDictionary = GameObject.Find("GroundDictionary").GetComponent<GroundDictionary>().groundDictionary;
        groundTileMap = GameObject.Find("Ground").GetComponent<Tilemap>();
        bigTNTDamage = 10000.0f;
        smallTNTDamage = 5000.0f;
        StartCoroutine(ExplodeTNT());
    }

    IEnumerator ExplodeTNT()
    {
        yield return new WaitForSeconds(TNTTimer);
        Vector3Int TNTGridPosition = groundTileMap.WorldToCell(this.transform.position);
        if (ToolID == 21)
        {
            int arraySize = bigTNTRange.GetLength(0);
            int offset = (arraySize - 1) / 2;

            for (int i = 0; i < arraySize; i++)
            {
                for (int j = 0; j < arraySize; j++)
                {
                    if (bigTNTRange[i, j] == 1)
                    {
                        Vector3Int groundGridPos = new Vector3Int(TNTGridPosition.x + i - offset, TNTGridPosition.y + j - offset, 0);
                        if (groundDictionary.ContainsKey(groundGridPos))
                        {
                            groundDictionary[groundGridPos].GetComponent<Ground>().takeDamage(bigTNTDamage);
                        }
                    }
                }
            }
        }
        else if(ToolID == 20)
        {
            int arraySize = smallTNTRange.GetLength(0);
            int offset = (arraySize - 1) / 2;

            for (int i =0; i<arraySize; i++)
            {
                for(int j =0; j<arraySize; j++)
                {
                    if (smallTNTRange[i,j] == 1)
                    {
                        Vector3Int groundGridPos = new Vector3Int(TNTGridPosition.x + i - offset, TNTGridPosition.y + j - offset, 0);
                        if (groundDictionary.ContainsKey(groundGridPos))
                        {
                            groundDictionary[groundGridPos].GetComponent<Ground>().takeDamage(smallTNTDamage);
                        }
                    }
                }
            }
        }
        Destroy(this.gameObject);
    }

    
}
