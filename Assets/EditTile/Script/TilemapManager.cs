using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    public Tilemap groundTilemap;
    public Tilemap railTilemap;
    public Tilemap ladderTilemap;
    public static TilemapManager instance = null;
    private void Awake()
    {
        instance = this;
    }
}

