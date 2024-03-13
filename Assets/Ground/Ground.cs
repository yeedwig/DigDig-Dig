using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ground : MonoBehaviour
{
    public float currentHealth,maxHealth,startBreakingHealth,almostBrokenHealth;
    public int groundLevel;
    public Tilemap groundTileMap;
    public Sprite[] groundSprites;
    public Sprite[] ruinSprites;
    public Sprite gangSprite;
    SpriteRenderer sr;
    public BoxCollider2D bc;
    public int x, y;
    public GameObject groundDictionaryObject;
    public bool isRuin = false;
    public bool gangInstalled = false;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        groundDictionaryObject = GameObject.Find("GroundDictionary");
        SelectGroundLevelHealthSpriteAndAddToDic();
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

    public void SelectGroundLevelHealthSpriteAndAddToDic()
    {
        groundTileMap = GameObject.Find("Ground").GetComponent<Tilemap>();
        Vector3Int groundGridPosition = groundTileMap.WorldToCell(this.transform.position);
        if (!isRuin)
        {
            if (groundGridPosition.y > -10)
            {
                groundLevel = 1;
                maxHealth = 200.0f;
            }
            else if (groundGridPosition.y > -30 && groundGridPosition.y <= -10)
            {
                groundLevel = 2;
                maxHealth = 300.0f;
            }
            else
            {
                groundLevel = 3;
                maxHealth = 300.0f;
            }
        }
        else
        {
            if (groundGridPosition.y > -10)
            {
                groundLevel = 1;
                maxHealth = 1000.0f;
            }
            else
            {
                groundLevel = 2;
                maxHealth = 3000.0f;
            }
        }
        
        currentHealth = maxHealth;
        startBreakingHealth = maxHealth * 0.7f;
        almostBrokenHealth = maxHealth * 0.3f;
        
        
        groundDictionaryObject.GetComponent<GroundDictionary>().AddToGroundDictionary(groundGridPosition,this.gameObject);
        groundDictionaryObject.GetComponent<RuinGenerator>().dictionaryInputDone = true;
    }

    

    public void ChangeSpriteByCurrentHealth()
    {
        if (gangInstalled)
        {
            sr.sprite = gangSprite;
            bc.enabled = false;
        }
        else {
            if (!isRuin)
            {
                if (currentHealth < 0)
                {
                    sr.sprite = null;
                    bc.enabled = false;
                }
                else if (currentHealth < almostBrokenHealth)
                {
                    sr.sprite = groundSprites[((groundLevel - 1) * 3) + 2];
                }
                else if (currentHealth < startBreakingHealth)
                {
                    sr.sprite = groundSprites[((groundLevel - 1) * 3) + 1];
                }
                else
                {
                    sr.sprite = groundSprites[((groundLevel - 1) * 3)];
                }
            }
            else
            {
                if (currentHealth < 0)
                {
                    sr.sprite = null;
                    bc.enabled = false;
                }
                else if (currentHealth < almostBrokenHealth)
                {
                    sr.sprite = ruinSprites[((groundLevel - 1) * 3) + 2];
                }
                else if (currentHealth < startBreakingHealth)
                {
                    sr.sprite = ruinSprites[((groundLevel - 1) * 3) + 1];
                }
                else
                {
                    sr.sprite = ruinSprites[((groundLevel - 1) * 3)];
                }
            }
        }
        
        
    }
}
