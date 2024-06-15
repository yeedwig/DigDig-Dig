using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap groundTilemap;
    public int mapWidthPerChunk, mapHeightPerChunk;
    public GameObject[] groundChunks;
    private GameObject[] chunks;
    [SerializeField] GameObject chunkController;
    public GameObject player;
    public GameObject saveload;
    // Start is called before the first frame update
    void Awake()
    {
        chunks = new GameObject[groundChunks.Length];
        if (SaveLoadManager.loaded)
        {
           player.transform.position = saveload.GetComponent<SaveLoadManager>().posTest();
        }
        CreateMap();
    }

    public void CreateMap()
    {
        int start, end;
        int pos = 2 * (-(int)player.transform.position.y / 50) + (int)player.transform.position.x / 50;
        if (pos % 2 == 0)
        {
            start = pos - 2;
            end = pos + 3;
        }
        else
        {
            start = pos - 3;
            end = pos + 2;
        }
        ChunkController CC = chunkController.GetComponent<ChunkController>();
        for (int i = 0; i < groundChunks.Length * 0.5; i++)
        {
            if (Random.Range(0, 2) == 0)
            {
                //(groundChunks[i * 2], groundChunks[i * 2 + 1]) = (groundChunks[i * 2 + 1], groundChunks[i * 2]);
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
                /*
                foreach(Transform t in chunks[index].transform)
                {
                    if (t.gameObject.GetComponent<Ground>() != null)
                    {
                        if (!GroundDictionary.instance.groundDictionary.ContainsKey(TilemapManager.instance.groundTilemap.WorldToCell(t.position)))
                        {
                            GroundDictionary.instance.groundDictionary.Add(TilemapManager.instance.groundTilemap.WorldToCell(t.position), t.gameObject);
                        }
                    }
                }
                */
                /*
                if(index >1 && (index < start || index > end))
                {
                    foreach (Transform t in chunks[index].transform)
                    {
                        t.gameObject.SetActive(false);
                    }
                    chunks[index].SetActive(false);
                }
                */
                index++;
                
            }
        }
        CC.chunks = chunks;
    }
}
