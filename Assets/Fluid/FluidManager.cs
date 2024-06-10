using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FluidManager : MonoBehaviour
{
    //�ܺ� ��ųʸ� �ҷ�����
    [SerializeField] GameObject groundDictionaryObject;
    private GroundDictionary groundDictionary;

    // ��ü ��ųʸ� ����
    public Dictionary<Vector3Int, int> waterDictionary = new Dictionary<Vector3Int, int>(); //��ǥ�� ������ ǥ��

    //��ü tilebase �迭
    [SerializeField] TileBase[] waterTiles;

    //��ü �帣�� �ð�
    [SerializeField] float waterSpreadDelay;
    private float waterSpreadTimer = 0;

    //�̱���
    public static FluidManager instance = null;
    // Start is called before the first frame update

    private void Awake()
    {
        waterDictionary = new Dictionary<Vector3Int, int>();
        instance = this;
    }
    void Start()
    {
        waterSpreadTimer = waterSpreadDelay;
        groundDictionary = groundDictionaryObject.GetComponent<GroundDictionary>();
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
    public void AddWaterBlock(Vector3Int pos,int level)
    {
        waterDictionary.Add(pos, level);
    }

    public void SpreadWater()
    {
        
        foreach (KeyValuePair<Vector3Int, int> water in waterDictionary)
        {
            if (!waterDictionary.ContainsKey(water.Key))
            {
                
                waterDictionary.Add(water.Key + new Vector3Int(0, -1, 0), 4);
            }
            
        }
        foreach (KeyValuePair<Vector3Int, int> water in waterDictionary)
        {
            TilemapManager.instance.waterTilemap.SetTile(water.Key, waterTiles[water.Value]);
        }
    }
}
