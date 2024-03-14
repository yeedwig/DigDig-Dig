using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public GameObject groundDictionaryObj;
    GroundDictionary groundDictionary;
    public Tilemap groundTilemap;
    public GameObject groundComponent;
    public GameObject[] ruins; //유적 프리펩 배열
    public int mapWidth, mapHeight; //맵 생성 크기
    public int[,] ruin1 = new int[7, 5]
    {
        {0,1,1,1,1},
        {0,1,1,1,1},
        {1,1,1,1,1},
        {1,1,1,1,1},
        {1,1,1,1,1},
        {1,1,1,1,1},
        {1,1,1,1,1}
    };
    public int[,] ruin2 = new int[5, 5]
    {
        {0,1,1,1,0},
        {1,1,1,1,1},
        {1,1,1,1,1},
        {1,1,1,1,1},
        {0,1,1,1,0}
    };
    // Start is called before the first frame update
    void Start()
    {
        groundDictionary = groundDictionaryObj.GetComponent<GroundDictionary>();
        CreateMap();
    }

    public void CreateMap()
    {
        for (int i = 0; i > -mapHeight; i--)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                GameObject ground = Instantiate(groundComponent);
                ground.transform.position = new Vector3(j + 0.5f, i + 0.5f, 0);
                groundDictionary.AddToGroundDictionary(new Vector3Int(j, i, 0), ground);
            }
        }
        CreateRuinOne();
        CreateRuinTwo();
    }
    public void CreateRuinOne()
    {
        int x, y, width, length;
        x = Random.Range(0, 80);
        y = Random.Range(-1, -3);
        length = ruin1.GetLength(0);
        width = ruin1.GetLength(1);
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (ruin1[i, j] == 1)
                {
                    Destroy(groundDictionary.FindFromGroundDictionary(new Vector3Int(x + j, y - i, 0)));
                    groundDictionary.DeleteFromGroundDictionary(new Vector3Int(x + j, y - i, 0));
                }
            }
        }
        GameObject test = Instantiate(ruins[0]);
        test.transform.position = groundTilemap.CellToWorld(new Vector3Int(x, y, 0));
    }

    public void CreateRuinTwo()
    {
        int x, y, width, length;
        x = Random.Range(0, 80);
        y = Random.Range(-11, -15);
        length = ruin2.GetLength(0);
        width = ruin2.GetLength(1);
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (ruin2[i, j] == 1)
                {
                    Destroy(groundDictionary.FindFromGroundDictionary(new Vector3Int(x + j, y - i, 0)));
                    groundDictionary.DeleteFromGroundDictionary(new Vector3Int(x + j, y - i, 0));
                }
            }
        }
        GameObject test = Instantiate(ruins[1]);
        test.transform.position = groundTilemap.CellToWorld(new Vector3Int(x, y, 0));
    }
}
