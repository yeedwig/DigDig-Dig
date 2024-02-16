using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ground : MonoBehaviour
{
    public float currentHealth,maxHealth,startBreakingHealth,almostBrokenHealth;
    public int groundLevel;
    public Tilemap groundTileMap;
    public Sprite[] groundSprites;
    SpriteRenderer sr;
    BoxCollider2D bc;
    public int x, y;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        SelectGroundLevelHealthSprite();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSpriteByCurrentHealth();
    }
    public void takeDamage(float damage)
    {
        currentHealth -= damage;
    }

    public void SelectGroundLevelHealthSprite()
    {
        groundTileMap = GameObject.Find("Ground").GetComponent<Tilemap>();
        Vector3Int groundGridPosition = groundTileMap.WorldToCell(this.transform.position);
        if(groundGridPosition.y>-5)
        {
            groundLevel = 1;
            maxHealth = 200.0f;
        }
        else if(groundGridPosition.y>-7&&groundGridPosition.y<=-5)
        {
            groundLevel = 2;
            maxHealth = 500.0f;
        }
        else
        {
            groundLevel = 3;
            maxHealth = 1000.0f;
        }
        currentHealth = maxHealth;
        startBreakingHealth = maxHealth * 0.7f;
        almostBrokenHealth = maxHealth * 0.3f;
        sr.sprite = groundSprites[((groundLevel-1)*3)];
    }

    public void ChangeSpriteByCurrentHealth()
    {
        if(currentHealth < 0)
        {
            sr.sprite = null;
            bc.enabled = false;
        }
        else if(currentHealth <almostBrokenHealth)
        {
            sr.sprite = groundSprites[((groundLevel - 1) * 3) + 2];
        }
        else if (currentHealth < startBreakingHealth)
        {
            sr.sprite = groundSprites[((groundLevel - 1) * 3)+1];
        }
    }
}
