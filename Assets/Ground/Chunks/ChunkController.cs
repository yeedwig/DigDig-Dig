using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkController : MonoBehaviour
{
    [SerializeField] GameObject player;
    
    public GameObject[] chunks;
    private int lastIndex;
    private int currentIndex;
    private int start;
    private int end;
    // Start is called before the first frame update
    void Start()
    {
        lastIndex = int.MinValue;
        
    }

    // Update is called once per frame
    void Update()
    {
        currentIndex = 2*(-(int)player.transform.position.y/50) + (int)player.transform.position.x/50;
        if (currentIndex != lastIndex)
        {
            lastIndex = currentIndex;
            ActivateMap(currentIndex);
        }
    }

    private void ActivateMap(int index)
    {
        if (index % 2 == 0)
        {
            start = index - 2;
            end = index + 3;
        }
        else
        {
            start = index - 3;
            end = index + 2;
        }
        for (int i=0;i<chunks.Length; i++)
        {
            if (i >= start && i <= end)
            {
                if (!chunks[i].activeSelf)
                {
                    chunks[i].SetActive(true);
                }
            }
            else
            {
                if (chunks[i].activeSelf)
                {
                    chunks[i].SetActive(false);
                }
            }
        }
    }
}
