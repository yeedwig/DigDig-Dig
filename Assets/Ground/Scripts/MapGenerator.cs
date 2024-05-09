using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap groundTilemap;
    public int mapWidthPerChunk, mapHeightPerChunk;
    public GameObject[] groundChunks;
    private GameObject[] chunks;
    [SerializeField] GameObject chunkController;
    // Start is called before the first frame update
    void Start()
    {
        chunks = new GameObject[groundChunks.Length];
        
        CreateMap();
    }

    public void CreateMap()
    {
        for (int i = 0; i < groundChunks.Length * 0.5; i++)
        {
            if (Random.Range(0, 2) == 0)
            {
                (groundChunks[i * 2], groundChunks[i * 2 + 1]) = (groundChunks[i * 2 + 1], groundChunks[i * 2]);
            }
        }
        int index = 0;
        for (int i = 0; i > -mapHeightPerChunk; i--)
        {
            for (int j = 0; j < mapWidthPerChunk; j++)
            {
                GameObject ground = Instantiate(groundChunks[index]);
                chunks[index] = ground;
                ground.transform.position = new Vector3(j * 50, i * 50, 0);
                index++;
            }
        }
        chunkController.GetComponent<ChunkController>().chunks = chunks;
    }
}
