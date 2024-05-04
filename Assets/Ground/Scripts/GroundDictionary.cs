using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundDictionary : MonoBehaviour
{
    public Dictionary<Vector3Int, GameObject> groundDictionary;
    
    public Tilemap groundTileMap;
    public GameObject player;

    void Awake()
    {
       groundDictionary =new Dictionary<Vector3Int, GameObject>();
    }
    

    public void AddToGroundDictionary(Vector3Int gridPosition, GameObject groundObject)
    {
        groundDictionary.Add(gridPosition, groundObject);
    }

    public void DeleteFromGroundDictionary(Vector3Int gridPosition)
    {
        groundDictionary.Remove(gridPosition);
    }

    public GameObject FindFromGroundDictionary(Vector3Int gridPosition)
    {
        return groundDictionary[gridPosition];
    }
    public void MapReset()
    {
        foreach(GameObject grounds in groundDictionary.Values)
        {
            Ground ground = grounds.GetComponent<Ground>();
            if (!ground.gangInstalled)
            {
                ground.currentHealth = ground.maxHealth;
                ground.bc.enabled = true;
                ground.ChangeSpriteByCurrentHealth();
            }  
        }
    }
}
