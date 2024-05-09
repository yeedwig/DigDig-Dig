using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject MapGenerator;
    private GameObject[] chunks;
    private int lastIndex;
    private int currentIndex;
    // Start is called before the first frame update
    void Start()
    {
        chunks = MapGenerator.GetComponent<MapGenerator>().groundChunks;
        lastIndex = -1000;
        //ActivateMap(lastIndex);
    }

    // Update is called once per frame
    void Update()
    {
        currentIndex = 2*(-(int)player.transform.position.y/50) + (int)player.transform.position.x/50;
        if (currentIndex != lastIndex)
        {
            lastIndex = currentIndex;
            //ActivateMap(currentIndex);
        }
    }

    private void ActivateMap(int index)
    {
        if (index % 2 == 0)
        {
            for(int i = index - 2; i < index + 4; i++)
            {
                if ((i >= 0 && i <= chunks.Length - 1) && !chunks[i].activeSelf)
                {
                    Debug.Log("activate(Â¦¼ö)");
                    chunks[i].SetActive(true);
                }
            }
        }
        else
        {
            for (int i = index - 3; i < index + 3; i++)
            {
                if ((i >= 0 && i <= chunks.Length - 1) && !chunks[i].activeSelf)
                {
                    Debug.Log("activate(È¦)");
                    chunks[i].SetActive(true);
                }
            }
        }
    }
}
