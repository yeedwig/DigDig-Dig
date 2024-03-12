using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundDictionary : MonoBehaviour
{
    public Dictionary<Vector3Int, GameObject> groundDictionary;
    public GameObject ruins;

    //기준점
    public GameObject point;
    public Tilemap groundTileMap;
    

    void Awake()
    {
       groundDictionary =new Dictionary<Vector3Int, GameObject>();
    }
    private void Update()
    {
        Debug.Log(groundDictionary.Count);
    }

    public void AddToGroundDictionary(Vector3Int gridPosition, GameObject groundObject)
    {
        groundDictionary.Add(gridPosition, groundObject);
    }
}
