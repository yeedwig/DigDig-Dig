using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ground : MonoBehaviour
{
    public float currentHealth,maxHealth; //���� ü��, �ִ� ü��, �Ӱ��� 2��
    public int groundLevel; //�� ����(���̿� ����)
    public bool gangInstalled = false; //������ ��ġ�Ǿ��°�
    //public bool structureInstalled = false; //��ġ�� �������� �ִ°�
    Vector3Int groundGridPosition;

    private GameObject groundDictionary;

    public Tilemap groundTileMap;
    public SpriteRenderer sr;
    public BoxCollider2D bc;


    private ItemDropManager itemDropManager;

    // �� �ı� Ȯ��
    int layermask;
    RaycastHit2D left;
    RaycastHit2D right;
    RaycastHit2D up;
    public Tilemap ladderTilemap;
    public Tilemap railTilemap;

    // ��ĭ���� Ȯ��
    public bool isBlank;

    //groundSO �ҷ�����
    private GroundComponents GC;


    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        itemDropManager = GameObject.Find("ItemDropManager").GetComponent<ItemDropManager>();
        layermask = 1 << LayerMask.NameToLayer("Structure");
        ladderTilemap = GameObject.Find("LadderTilemap").GetComponent<Tilemap>();
        railTilemap = GameObject.Find("RailTilemap").GetComponent<Tilemap>();
        groundDictionary = GameObject.Find("GroundDictionary");
        GC = GameObject.Find("GroundComponents").GetComponent<GroundComponents>();

        SelectGroundLevelHealth();
        ChangeSpriteByCurrentHealth();
    }

    public void takeDamage(float damage) //������ �ִ� �Լ�
    {
        currentHealth -= damage;
        ChangeSpriteByCurrentHealth();
        itemDropManager.GetItem(GC.groundSO[groundLevel-1]);
    }

    public void MonsterDamage(float damage)
    {
        currentHealth -= damage;
        ChangeSpriteByCurrentHealth();
    }

    public void SelectGroundLevelHealth()
    {
        groundTileMap = GameObject.Find("Ground").GetComponent<Tilemap>();
        groundGridPosition = groundTileMap.WorldToCell(this.transform.position);

        if (!isBlank)
        {
            if (groundGridPosition.y > -50)
            {
                groundLevel = 1;
                maxHealth = 100.0f;
            }
            else if (groundGridPosition.y > -100 && groundGridPosition.y <= -50)
            {
                groundLevel = 2;
                maxHealth = 500.0f;
            }
            else if (groundGridPosition.y > -150 && groundGridPosition.y <= -100)
            {
                groundLevel = 3;
                maxHealth = 500.0f;
            }
            else if (groundGridPosition.y > -200 && groundGridPosition.y <= -150)
            {
                groundLevel = 4;
                maxHealth = 500.0f;
            }
            else if (groundGridPosition.y > -250 && groundGridPosition.y <= -200)
            {
                groundLevel = 5;
                maxHealth = 500.0f;
            }
            else if (groundGridPosition.y > -300 && groundGridPosition.y <= -250)
            {
                groundLevel = 6;
                maxHealth = 500.0f;
            }
            else if (groundGridPosition.y > -350 && groundGridPosition.y <= -300)
            {
                groundLevel = 7;
                maxHealth = 500.0f;
            }
            else if (groundGridPosition.y > -400 && groundGridPosition.y <= -350)
            {
                groundLevel = 8;
                maxHealth = 500.0f;
            }
            else if (groundGridPosition.y > -450 && groundGridPosition.y <= -400)
            {
                groundLevel = 9;
                maxHealth = 500.0f;
            }
            else
            {
                groundLevel = 10;
                maxHealth = 1000.0f;
            }

            currentHealth = maxHealth;
        }
        groundDictionary.GetComponent<GroundDictionary>().AddToGroundDictionary(groundGridPosition,this.gameObject);
    }

    

    public void ChangeSpriteByCurrentHealth()
    {
        if(!isBlank)
        {
            if (currentHealth < 0.001)
            {
                CheckNearStructure();
                sr.sprite = null;
                bc.enabled = false;
            }
            else if (currentHealth < maxHealth * 0.3f) //���� �μ���
            {
                sr.sprite = GC.groundSO[groundLevel-1].groundSprites[2];
                //groundSO[groundLevel - 1].groundSprites[2];//groundSprites[((groundLevel - 1) * 3) + 2];
            }
            else if (currentHealth < maxHealth * 0.7f) //�μ����� ����
            {
                sr.sprite = GC.groundSO[groundLevel - 1].groundSprites[1];//groundSprites[((groundLevel - 1) * 3) + 1];

            }
            else
            {
                sr.sprite = GC.groundSO[groundLevel - 1].groundSprites[0];//groundSprites[((groundLevel - 1) * 3)];
            }
        }
        
    }

    private void CheckNearStructure()
    {
        left = Physics2D.Raycast(this.transform.position, new Vector2(-1, 0), 0.65f, layermask);
        right = Physics2D.Raycast(this.transform.position, new Vector2(1, 0), 0.65f, layermask);
        up = Physics2D.Raycast(this.transform.position, new Vector2(0, 1), 0.65f, layermask);
        if (left.collider != null)
        {
            ladderTilemap.SetTile(groundGridPosition+new Vector3Int(-1,0,0),null);
        }
        if(right.collider != null)
        {
            ladderTilemap.SetTile(groundGridPosition + new Vector3Int(1, 0, 0), null);
        }
        if (up.collider != null)
        {
            railTilemap.SetTile(groundGridPosition + new Vector3Int(0, 1, 0), null);
        }
    }
}
