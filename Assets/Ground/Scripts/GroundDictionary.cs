using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundDictionary : MonoBehaviour
{
    public Dictionary<Vector3Int, GameObject> groundDictionary;
    
    public Tilemap groundTileMap;
    public GameObject player;
    public static GroundDictionary instance = null;

    void Awake()
    {
       groundDictionary =new Dictionary<Vector3Int, GameObject>();
       instance = this;
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
        foreach(KeyValuePair<Vector3Int , GameObject> pair in groundDictionary)
        {
            Ground ground = pair.Value.GetComponent<Ground>();
            if (!GangController.instance.gangDictionary.ContainsKey(pair.Key) && !ground.isBlank)
            {
                ground.currentHealth = ground.maxHealth;
                ground.bc.enabled = true;
                ground.ChangeSpriteByCurrentHealth();
            }
        }
    }
}
