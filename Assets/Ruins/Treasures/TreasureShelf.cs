using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureShelf : MonoBehaviour
{

    [SerializeField] public GameObject[] Treasures;
    [SerializeField] public bool[] TreasuresFound;

    // Update is called once per frame
    private void Start()
    {
        for(int i=0; i< TreasuresFound.Length; i++)
        {
            TreasuresFound[i] = false;
        }
    }
    void Update()
    {
        for(int i=0; i<TreasuresFound.Length;i++)
        {
            if (TreasuresFound[i] == true)
            {
                Treasures[i].gameObject.SetActive(true);
            }
        }
    }
}
