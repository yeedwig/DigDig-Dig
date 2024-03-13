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
    public GameObject player;

    void Awake()
    {
       groundDictionary =new Dictionary<Vector3Int, GameObject>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Vector3Int groundGridPosition = groundTileMap.WorldToCell(player.transform.position);
            groundDictionary[groundGridPosition].GetComponent<Ground>().gangInstalled = true;
            groundDictionary[groundGridPosition].GetComponent<Ground>().ChangeSpriteByCurrentHealth();


        }
    }

    public void AddToGroundDictionary(Vector3Int gridPosition, GameObject groundObject)
    {
        groundDictionary.Add(gridPosition, groundObject);
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
