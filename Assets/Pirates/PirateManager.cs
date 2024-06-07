using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateManager : MonoBehaviour
{
    public GameObject[] MapPieces;
    public bool[] mapPiecesFound;
    public bool shovelGiven;
    // Start is called before the first frame update
    void Start()
    {
        shovelGiven = false;
        mapPiecesFound = new bool[8];
        for(int i=0; i<8; i++)
        {
            mapPiecesFound[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        mapPiecesCheck();
    }

    void mapPiecesCheck()
    {
        for(int i=0; i<8; i++)
        {
            MapPieces[i].SetActive(mapPiecesFound[i]);
        }
    }

    public void mapPieceEarned(int piece)
    {
        mapPiecesFound[piece] = true;
    }
}
