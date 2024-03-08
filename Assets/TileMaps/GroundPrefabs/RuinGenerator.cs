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
    public GameObject[] ruins;
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
    // Start is called before the first frame update
    void Start()
    {
        groundDictionary = groundDictionaryObj.GetComponent<GroundDictionary>().groundDictionary; 
    }
    private void Update()
    {
        if (dictionaryInputDone)
        {
            dictionaryInputDone = false;
            CreateRuins();
        }
    }

    public void CreateRuins()
    {
        CreateRuinOne();
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
                    
                }
            }
        }
        GameObject test = Instantiate(ruins[0]);
        test.transform.position = ground.CellToWorld(new Vector3Int(x, y, 0));
    }
}
