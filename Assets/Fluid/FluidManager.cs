using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class FluidManager : MonoBehaviour
{
    //¿ÜºÎ µñ¼Å³Ê¸® ºÒ·¯¿À±â
    [SerializeField] GameObject groundDictionaryObject;
    private GroundDictionary groundDictionary;

    // ¾×Ã¼ µñ¼Å³Ê¸® ¼±¾ð
    public Dictionary<Vector3Int, int> waterBlockDictionary; //ÁÂÇ¥¿Í ¼öÀ§¸¦ Ç¥½Ã
    public Dictionary<Vector3Int, int> waterMapDictionary;

    //¾×Ã¼ tilebase ¹è¿­
    [SerializeField] TileBase[] waterTiles;

    //¾×Ã¼ Èå¸£´Â ½Ã°£
    [SerializeField] float waterSpreadDelay;
    private float waterSpreadTimer = 0;

    private int waterMask;

    //½Ì±ÛÅæ
    public static FluidManager instance = null;
    // Start is called before the first frame update

    int test = 0;

    private void Awake()
    {
        waterBlockDictionary = new Dictionary<Vector3Int, int>();
        waterMapDictionary = new Dictionary<Vector3Int, int>();
        instance = this;
    }
    void Start()
    {
        waterSpreadTimer = waterSpreadDelay;
        groundDictionary = groundDictionaryObject.GetComponent<GroundDictionary>();
        waterMask = 1 << LayerMask.NameToLayer("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSpreadTimer();
        if (waterSpreadTimer > waterSpreadDelay)
        {
            SpreadWater();
            waterSpreadTimer = 0;
        }
    }

    private void ChangeSpreadTimer()
    {
        waterSpreadTimer += Time.deltaTime;
    }

    public void SpreadWater()
    {
        waterMapDictionary.Clear();
        foreach (KeyValuePair<Vector3Int, int> water in waterBlockDictionary)
        {
            //AddToWaterMap(water.Key, 4);
            StartCoroutine(AddWaterToMap(water.Key, 4));
        }
        ShowWater();
    }

    IEnumerator AddWaterToMap(Vector3Int pos, int level)
    {
        if (!waterMapDictionary.ContainsKey(pos))
        {
            waterMapDictionary.Add(pos, level);
        }
        else
        {
            waterMapDictionary[pos] = Mathf.Max(waterMapDictionary[pos], level);
        }

        Collider2D waterCheck = Physics2D.OverlapCircle(((Vector2Int)pos) + new Vector2(0.5f, -0.5f), 0.4f, waterMask);
        if (waterCheck == null)
        {
            AddToWaterMap(pos + new Vector3Int(0, -1, 0), 4);
        }

        else if (waterCheck != null && level > 0)
        {
            waterCheck = Physics2D.OverlapCircle(((Vector2Int)pos) + new Vector2(-0.5f, 0.5f), 0.4f, waterMask);
            if (waterCheck == null)
            {
                AddToWaterMap(pos + new Vector3Int(-1, 0, 0), level - 1);
            }
            waterCheck = Physics2D.OverlapCircle(((Vector2Int)pos) + new Vector2(1.5f, 0.5f), 0.4f, waterMask);
            if (waterCheck == null)
            {
                AddToWaterMap(pos + new Vector3Int(1, 0, 0), level - 1);
            }
        }
        yield return null;
    }
    public void AddToWaterMap(Vector3Int pos, int level)
    {
        if(!waterMapDictionary.ContainsKey(pos)) {
            waterMapDictionary.Add(pos, level);
        }
        else
        {
            waterMapDictionary[pos] = Mathf.Max(waterMapDictionary[pos],level);
        }
        
        Collider2D waterCheck = Physics2D.OverlapCircle(((Vector2Int)pos) + new Vector2(0.5f, -0.5f), 0.4f, waterMask);
        if (waterCheck == null)
        {
            AddToWaterMap(pos + new Vector3Int(0, -1, 0), 4);
        }
        
        else if(waterCheck != null && level>0)
        {
            waterCheck = Physics2D.OverlapCircle(((Vector2Int)pos) + new Vector2(-0.5f, 0.5f), 0.4f, waterMask);
            if (waterCheck==null)
            {
                AddToWaterMap(pos + new Vector3Int(-1,0, 0), level-1);
            }
            waterCheck = Physics2D.OverlapCircle(((Vector2Int)pos) + new Vector2(1.5f, 0.5f), 0.4f, waterMask);
            if (waterCheck == null)
            {
                AddToWaterMap(pos + new Vector3Int(1, 0, 0), level - 1);
            }
        }
        
    }

    public void ShowWater()
    {
        TilemapManager.instance.waterTilemap.ClearAllTiles();
        foreach (KeyValuePair<Vector3Int, int> water in waterMapDictionary)
        {
            TilemapManager.instance.waterTilemap.SetTile(water.Key, waterTiles[water.Value]);
        }
    }
    
}
