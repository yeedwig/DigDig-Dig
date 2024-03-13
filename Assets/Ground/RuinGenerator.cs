using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RuinGenerator : MonoBehaviour
{
    public Dictionary<Vector3Int, GameObject> groundDictionary;
    public GameObject groundDictionaryObj;
    public bool dictionaryInputDone=false;
    public Tilemap ground;
    public GameObject groundComponent;
    public GameObject[] ruins;
    public int mapWidth,mapHeight;
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
        groundDictionary = groundDictionaryObj.GetComponent<GroundDictionary>().groundDictionary;
        CreateMap();
        
    }

    
    IEnumerator CreateRuins()
    {
        while (!dictionaryInputDone)
        {
            yield return null;
        }
        CreateRuinOne();
        CreateRuinTwo();
    }

    public void CreateMap()
    {
        for(int i = 0; i > -mapHeight; i--)
        {
            for(int j = 0; j < mapWidth; j++)
            {
                GameObject ground = Instantiate(groundComponent);
                ground.transform.position = new Vector3(j+0.5f, i+0.5f, 0);
            }
        }
        StartCoroutine(CreateRuins());
    }
    public void CreateRuinOne()
    {
        int x, y, width, length;
        x = Random.Range(0, 8);
        y = Random.Range(-1, -3);
        length = ruin1.GetLength(0);
        width = ruin1.GetLength(1);
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (ruin1[i, j] == 1)
                {
                    Destroy(groundDictionary[new Vector3Int(x + j, y - i, 0)]);
                    groundDictionary.Remove(new Vector3Int(x + j, y - i, 0));
                }
            }
        }
        GameObject test = Instantiate(ruins[0]);
        test.transform.position = ground.CellToWorld(new Vector3Int(x, y, 0));
    }

    public void CreateRuinTwo()
    {
        int x, y, width, length;
        x = Random.Range(0, 8);
        y = Random.Range(-11, -15);
        length = ruin2.GetLength(0);
        width = ruin2.GetLength(1);
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (ruin2[i, j] == 1)
                {
                    Destroy(groundDictionary[new Vector3Int(x + j, y - i, 0)]);
                    groundDictionary.Remove(new Vector3Int(x + j, y - i, 0));
                }
            }
        }
        GameObject test = Instantiate(ruins[1]);
        test.transform.position = ground.CellToWorld(new Vector3Int(x, y, 0));
    }
}
