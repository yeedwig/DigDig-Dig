using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ground : MonoBehaviour
{
    public float health;
    public int groundLevel;
    public Tilemap groundTileMap;
    public Sprite[] groundSprites;
    SpriteRenderer sr;
    public int x, y;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        SelectGroundLevelHealthSprite();
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 0)
        {
            this.gameObject.SetActive(false);
        }
    }
    public void takeDamage(float damage)
    {
        health -= damage;
    }

    public void SelectGroundLevelHealthSprite()
    {
        groundTileMap = GameObject.Find("Ground").GetComponent<Tilemap>();
        Vector3Int groundGridPosition = groundTileMap.WorldToCell(this.transform.position);
        if(groundGridPosition.y>-5)
        {
            groundLevel = 1;
            health = 300.0f;
        }
        else if(groundGridPosition.y>-7&&groundGridPosition.y<=-5)
        {
            groundLevel = 2;
            health = 500.0f;
        }
        else
        {
            groundLevel = 3;
            health = 1000.0f;
        }
        sr.sprite = groundSprites[groundLevel-1];
    }
}
