using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
//using static UnityEditor.PlayerSettings;

public class FluidManager : MonoBehaviour
{
    //외부 딕셔너리 불러오기
    [SerializeField] GameObject groundDictionaryObject;
    private GroundDictionary groundDictionary;

    // 액체 딕셔너리 선언
    public Dictionary<Vector3Int, int> waterBlockDictionary; //좌표와 수위를 표시
    public Dictionary<Vector3Int, int> waterMapDictionary;

    //액체 tilebase 배열
    [SerializeField] TileBase[] waterTiles;

    //액체 흐르는 시간
    [SerializeField] float waterSpreadDelay;
    private float waterSpreadTimer = 0;
    private int waterOrder = 0;

    private int waterMask;

    //용암 관련
    public Dictionary<Vector3Int, int> lavaBlockDictionary; //좌표와 수위를 표시
    public Dictionary<Vector3Int, int> lavaMapDictionary;
    [SerializeField] float lavaSpreadDelay;
    private float lavaSpreadTimer = 0;
    private int lavaOrder = 0;
    [SerializeField] TileBase[] lavaTiles;
    private int lavaMask;


    //가스 관련
    public Dictionary<Vector3Int, int> gasBlockDictionary; //좌표와 수위를 표시
    public Dictionary<Vector3Int, int> gasMapDictionary;
    [SerializeField] float gasSpreadDelay;
    private float gasSpreadTimer = 0;
    private int gasOrder = 0;
    [SerializeField] TileBase[] gasTiles;
    private int gasMask;
    //싱글톤
    public static FluidManager instance = null;
    // Start is called before the first frame update

    int test = 0;

    private void Awake()
    {
        waterBlockDictionary = new Dictionary<Vector3Int, int>();
        waterMapDictionary = new Dictionary<Vector3Int, int>();
        lavaBlockDictionary = new Dictionary<Vector3Int, int>();
        lavaMapDictionary = new Dictionary<Vector3Int, int>();
        gasBlockDictionary = new Dictionary<Vector3Int, int>();
        gasMapDictionary = new Dictionary<Vector3Int, int>();
        instance = this;
    }
    void Start()
    {
        waterSpreadTimer = waterSpreadDelay;
        lavaSpreadTimer = lavaSpreadDelay;
        groundDictionary = groundDictionaryObject.GetComponent<GroundDictionary>();
        waterMask = 1 << LayerMask.NameToLayer("Ground");
        lavaMask = 1 << LayerMask.NameToLayer("Ground");
        gasMask = 1 << LayerMask.NameToLayer("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSpreadTimer();
        WaterCheck();
        LavaCheck();
        GasCheck();
    }

    private void ChangeSpreadTimer()
    {
        waterSpreadTimer += Time.deltaTime;
        lavaSpreadTimer += Time.deltaTime;
        gasSpreadTimer += Time.deltaTime;
    }

    private void WaterCheck()
    {
        if (waterSpreadTimer > waterSpreadDelay-0.2f && waterOrder==0)
        {
            waterMapDictionary.Clear();
            waterOrder++;
        }
        else if(waterSpreadTimer > waterSpreadDelay - 0.1f && waterOrder == 1){
            SpreadWater();
            waterOrder++;
        }
        else if(waterSpreadTimer > waterSpreadDelay && waterOrder == 2)
        {
            ShowWater();
            waterOrder = 0;
            waterSpreadTimer = 0;
        }
    }

    public void SpreadWater()
    {
        for(int i=0; i<waterBlockDictionary.Count; i++)
        {
            waterBlockDictionary[waterBlockDictionary.Keys.ToList()[i]]++;
            StartCoroutine(AddWaterToMap(waterBlockDictionary.Keys.ToList()[i], waterBlockDictionary.Keys.ToList()[i], waterTiles.Length-1));
        }
    }

    IEnumerator AddWaterToMap(Vector3Int origin,Vector3Int pos, int level)
    {
        int previous = waterBlockDictionary[origin];
        if (!waterMapDictionary.ContainsKey(pos))
        {
            waterMapDictionary.Add(pos, level);
        }
        else
        {
            waterMapDictionary[pos] = Mathf.Max(waterMapDictionary[pos], level);
        }

        while(previous == waterBlockDictionary[origin])
        {
            
            yield return null;
        }

        Collider2D waterCheck = Physics2D.OverlapCircle(((Vector2Int)pos) + new Vector2(0.5f, -0.5f), 0.4f, waterMask);
        if (waterCheck == null)
        {
            StartCoroutine(AddWaterToMap(origin,pos + new Vector3Int(0, -1, 0), waterTiles.Length-1));
        }

        else if (waterCheck != null && level > 0)
        {
            waterCheck = Physics2D.OverlapCircle(((Vector2Int)pos) + new Vector2(-0.5f, 0.5f), 0.4f, waterMask);
            if (waterCheck == null)
            {
                StartCoroutine(AddWaterToMap(origin,pos + new Vector3Int(-1, 0, 0), level - 1));
            }
            waterCheck = Physics2D.OverlapCircle(((Vector2Int)pos) + new Vector2(1.5f, 0.5f), 0.4f, waterMask);
            if (waterCheck == null)
            {
                StartCoroutine(AddWaterToMap(origin,pos + new Vector3Int(1, 0, 0), level - 1));
            }
        }
        yield return null;
    }
    
    public void ShowWater()
    {
        TilemapManager.instance.waterTilemap.ClearAllTiles();
        foreach (KeyValuePair<Vector3Int, int> water in waterMapDictionary)
        {
            TilemapManager.instance.waterTilemap.SetTile(water.Key, waterTiles[water.Value]);
        }
    }

    private void LavaCheck()
    {
        if (lavaSpreadTimer > lavaSpreadDelay - 0.2f && lavaOrder == 0)
        {
            lavaMapDictionary.Clear();
            lavaOrder++;
        }
        else if (lavaSpreadTimer > lavaSpreadDelay - 0.1f && lavaOrder == 1)
        {
            SpreadLava();
            lavaOrder++;
        }
        else if (lavaSpreadTimer > lavaSpreadDelay && lavaOrder == 2)
        {
            ShowLava();
            lavaOrder = 0;
            lavaSpreadTimer = 0;
        }
    }

    public void SpreadLava()
    {
        for (int i = 0; i < lavaBlockDictionary.Count; i++)
        {
            lavaBlockDictionary[lavaBlockDictionary.Keys.ToList()[i]]++;
            StartCoroutine(AddLavaToMap(lavaBlockDictionary.Keys.ToList()[i], lavaBlockDictionary.Keys.ToList()[i], lavaTiles.Length-1));
        }
    }

    IEnumerator AddLavaToMap(Vector3Int origin, Vector3Int pos, int level)
    {
        int previous = lavaBlockDictionary[origin];
        if (!lavaMapDictionary.ContainsKey(pos))
        {
            lavaMapDictionary.Add(pos, level);
        }
        else
        {
            lavaMapDictionary[pos] = Mathf.Max(lavaMapDictionary[pos], level);
        }

        while (previous == lavaBlockDictionary[origin])
        {
            yield return null;
        }

        Collider2D lavaCheck = Physics2D.OverlapCircle(((Vector2Int)pos) + new Vector2(0.5f, -0.5f), 0.4f, lavaMask);
        if (lavaCheck == null)
        {
            StartCoroutine(AddLavaToMap(origin, pos + new Vector3Int(0, -1, 0), lavaTiles.Length-1));
        }

        else if (lavaCheck != null && level > 0)
        {
            lavaCheck = Physics2D.OverlapCircle(((Vector2Int)pos) + new Vector2(-0.5f, 0.5f), 0.4f, lavaMask);
            if (lavaCheck == null)
            {
                StartCoroutine(AddLavaToMap(origin, pos + new Vector3Int(-1, 0, 0), level - 1));
            }
            lavaCheck = Physics2D.OverlapCircle(((Vector2Int)pos) + new Vector2(1.5f, 0.5f), 0.4f, lavaMask);
            if (lavaCheck == null)
            {
                StartCoroutine(AddLavaToMap(origin, pos + new Vector3Int(1, 0, 0), level - 1));
            }
        }
        yield return null;
    }

    public void ShowLava()
    {
        TilemapManager.instance.lavaTilemap.ClearAllTiles();
        foreach (KeyValuePair<Vector3Int, int> lava in lavaMapDictionary)
        {
            TilemapManager.instance.lavaTilemap.SetTile(lava.Key, lavaTiles[lava.Value]);
        }
    }

    private void GasCheck()
    {
        if (gasSpreadTimer > gasSpreadDelay - 0.2f && gasOrder == 0)
        {
            gasMapDictionary.Clear();
            gasOrder++;
        }
        else if (gasSpreadTimer > gasSpreadDelay - 0.1f && gasOrder == 1)
        {
            SpreadGas();
            gasOrder++;
        }
        else if (gasSpreadTimer > gasSpreadDelay && gasOrder == 2)
        {
            ShowGas();
            gasOrder = 0;
            gasSpreadTimer = 0;
        }
    }

    public void SpreadGas()
    {
        for (int i = 0; i < gasBlockDictionary.Count; i++)
        {
            gasBlockDictionary[gasBlockDictionary.Keys.ToList()[i]]++;
            StartCoroutine(AddGasToMap(gasBlockDictionary.Keys.ToList()[i], gasBlockDictionary.Keys.ToList()[i], gasTiles.Length - 1));
        }
    }

    IEnumerator AddGasToMap(Vector3Int origin, Vector3Int pos, int level)
    {
        int previous = gasBlockDictionary[origin];
        if (!gasMapDictionary.ContainsKey(pos))
        {
            gasMapDictionary.Add(pos, level);
        }
        else
        {
            gasMapDictionary[pos] = Mathf.Max(gasMapDictionary[pos], level);
        }

        while (previous == gasBlockDictionary[origin])
        {
            yield return null;
        }

        Collider2D gasCheck = Physics2D.OverlapCircle(((Vector2Int)pos) + new Vector2(0.5f, +1.5f), 0.4f, gasMask);
        if (gasCheck == null)
        {
            StartCoroutine(AddGasToMap(origin, pos + new Vector3Int(0, +1, 0), gasTiles.Length-1));
        }

        else if (gasCheck != null && level > 0)
        {
            gasCheck = Physics2D.OverlapCircle(((Vector2Int)pos) + new Vector2(-0.5f, 0.5f), 0.4f, gasMask);
            if (gasCheck == null)
            {
                StartCoroutine(AddGasToMap(origin, pos + new Vector3Int(-1, 0, 0), level - 1));
            }
            gasCheck = Physics2D.OverlapCircle(((Vector2Int)pos) + new Vector2(1.5f, 0.5f), 0.4f, gasMask);
            if (gasCheck == null)
            {
                StartCoroutine(AddGasToMap(origin, pos + new Vector3Int(1, 0, 0), level - 1));
            }
        }
        yield return null;
    }

    public void ShowGas()
    {
        TilemapManager.instance.gasTilemap.ClearAllTiles();
        foreach (KeyValuePair<Vector3Int, int> gas in gasMapDictionary)
        {
            TilemapManager.instance.gasTilemap.SetTile(gas.Key, gasTiles[gas.Value]);
        }
    }
}
