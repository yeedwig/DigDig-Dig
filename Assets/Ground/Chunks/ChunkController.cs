using System;
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
        lastIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentIndex = 2*(-(int)player.transform.position.y/50) + (int)player.transform.position.x/50;
        if (currentIndex != lastIndex)
        {
            Debug.Log("please no");
            lastIndex = currentIndex;
            ActivateMap(currentIndex);
        }
    }

    public void ActivateMap(int index)
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
        for (int i=2;i<chunks.Length; i++)
        {
            if (i >= start && i <= end)
            {
                if (!chunks[i].activeSelf)
                {
                    StartCoroutine(ActivateChunk(chunks[i]));
                }
            }
            else
            {
                if (chunks[i].activeSelf)
                {
                    StartCoroutine(DisableChunk(chunks[i]));
                }
            }
        }
    }

    public IEnumerator ActivateChunk(GameObject chunk)
    {
        foreach (Transform ground in chunk.transform)
        {
            ground.gameObject.SetActive(true);
            yield return null;
        }
        chunk.SetActive(true);
        yield return null;
    }
    public IEnumerator DisableChunk(GameObject chunk)
    {
        foreach (Transform ground in chunk.transform)
        {
            ground.gameObject.SetActive(false);
            yield return null;
        }
        chunk.SetActive(false);
        yield return null;
    }
}
