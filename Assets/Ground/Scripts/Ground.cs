using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ground : MonoBehaviour
{
    public float currentHealth,maxHealth,startBreakingHealth,almostBrokenHealth; //현재 체력, 최대 체력, 임계점 2개
    public int groundLevel; //땅 레벨(깊이에 따라)
    public bool isRuin = false; //유적인가
    public bool gangInstalled = false; //갱도가 설치되었는가
    public int[] groundMaxPerLevel; //지하 땅 레벨 y 좌표
    public float breakThreshold1, breakThreshold2;

    private Dictionary<Vector3Int, GameObject> groundDictionary;

    public Tilemap groundTileMap;
    public SpriteRenderer sr;
    public BoxCollider2D bc;

    public Sprite[] groundSprites; //땅 스프라이트 (3개 단위로 평소, 조금 부서짐, 거의 부서짐)
    public Sprite[] ruinSprites;
    public Sprite gangSprite;
    
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        SelectGroundLevelHealth();
        ChangeSpriteByCurrentHealth();
    }

    public void takeDamage(float damage) //데미지 주는 함수
    {
        currentHealth -= damage;
        ChangeSpriteByCurrentHealth() ;
    }

    public void SelectGroundLevelHealth()
    {
        groundTileMap = GameObject.Find("Ground").GetComponent<Tilemap>();
        Vector3Int groundGridPosition = groundTileMap.WorldToCell(this.transform.position);
        if (!isRuin)
        {
            if (groundGridPosition.y > groundMaxPerLevel[0])
            {
                groundLevel = 1;
                maxHealth = 200.0f;
            }
            else if (groundGridPosition.y > groundMaxPerLevel[1] && groundGridPosition.y <= groundMaxPerLevel[0])
            {
                groundLevel = 2;
                maxHealth = 500.0f;
            }
            else
            {
                groundLevel = 3;
                maxHealth = 1000.0f;
            }
        }
        else
        {
            if (groundGridPosition.y > groundMaxPerLevel[0])
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
        startBreakingHealth = maxHealth * breakThreshold1;
        almostBrokenHealth = maxHealth * breakThreshold2;

        if (isRuin)
        {
            groundDictionary = GameObject.Find("GroundDictionary").GetComponent<GroundDictionary>().groundDictionary;
            groundDictionary.Add(groundGridPosition, this.gameObject);
        }

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
                    //얘는 한번만 실행됨
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
