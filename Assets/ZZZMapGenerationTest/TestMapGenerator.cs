using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestMapGenerator : MonoBehaviour
{
    public GameObject groundDictionaryObj;
    GroundDictionary groundDictionary;
    public Tilemap groundTilemap;
    public GameObject groundComponent;
    public GameObject groundChunk;
    public int mapWidth, mapHeight; //¸Ê »ý¼º Å©±â
    public int mapWidthPerChunk, mapHeightPerChunk;
    public bool isOldVersion;
    void Start()
    {
        groundDictionary = groundDictionaryObj.GetComponent<GroundDictionary>();
        CreateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateMap()
    {
        if (isOldVersion) 
        {
            for (int i = 0; i > -mapHeight; i--)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    GameObject ground = Instantiate(groundComponent);
                    ground.transform.position = new Vector3(j + 0.5f, i + 0.5f, 0);
                    //groundDictionary.AddToGroundDictionary(new Vector3Int(j, i, 0), ground);
                }
            }
        }
        else
        {
            for (int i = 0; i > -mapHeightPerChunk; i--)
            {
                for (int j = 0; j < mapWidthPerChunk; j++)
                {
                    GameObject ground = Instantiate(groundChunk);
                    ground.transform.position = new Vector3(j*50, i*50, 0);
                    //groundDictionary.AddToGroundDictionary(new Vector3Int(j, i, 0), ground);
                }
            }
        }
        
    }
}
