using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GangController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TileBase gang;
    public Dictionary<Vector3Int, bool> gangDictionary;

    [SerializeField] GameObject gangLightParent;
    [SerializeField] GameObject gangLight;

    [SerializeField] int gangLightPoolSize;
    private int gangPoolCounter = 0;

    public Dictionary<Vector3Int, int> gangLightPoolDictionary;

    [SerializeField] GameObject player;
    [SerializeField] float height,width;


    public static GangController instance = null;
    
    private void Awake()
    {
        gangDictionary = new Dictionary<Vector3Int, bool>();
        gangLightPoolDictionary = new Dictionary<Vector3Int, int>();
        instance = this;
    }

    private void Start()
    {
        for(int i=0;i<gangLightPoolSize; i++)
        {
            GameObject temp = Instantiate(gangLight) as GameObject;
            temp.transform.SetParent(gangLightParent.transform, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ControlGangLight();
    }

    public void CreateGang(Vector3Int pos)
    {
        if(!gangDictionary.ContainsKey(pos))
        {
            TilemapManager.instance.gangTilemap.SetTile(pos, gang);
            gangDictionary.Add(pos, false);
        }
        
    }
    public void DestroyGang(Vector3Int pos)
    {
        TilemapManager.instance.gangTilemap.SetTile(pos, null);
        gangDictionary.Remove(pos);
        if (gangLightPoolDictionary.ContainsKey(pos) )
        {
            gangLightParent.transform.GetChild(gangLightPoolDictionary[pos]).gameObject.SetActive(false);
            gangLightPoolDictionary.Remove(pos);
        }
    }

    public void ControlGangLight()
    {
        for(int i=0;i<gangDictionary.Count;i++)
        {
            if (CheckInBoundary(gangDictionary.Keys.ToList()[i]) && !gangDictionary[gangDictionary.Keys.ToList()[i]]) //나중에 범위 안으로 변경
            {
                while (true)
                {  
                    if (gangLightParent.transform.GetChild(gangPoolCounter).gameObject.activeSelf)
                    {
                        CounterIncrease();
                    }
                    else
                    {
                        gangLightParent.transform.GetChild(gangPoolCounter).gameObject.SetActive(true);
                        gangLightParent.transform.GetChild(gangPoolCounter).position = gangDictionary.Keys.ToList()[i]+new Vector3(0.5f,0.5f,0);
                        gangLightPoolDictionary.Add(gangDictionary.Keys.ToList()[i], gangPoolCounter);
                        break;
                    }
                }
                gangDictionary[gangDictionary.Keys.ToList()[i]] = true;
            }
            else if(!CheckInBoundary(gangDictionary.Keys.ToList()[i]) && gangDictionary[gangDictionary.Keys.ToList()[i]])
            {
                gangLightParent.transform.GetChild(gangLightPoolDictionary[gangDictionary.Keys.ToList()[i]]).gameObject.SetActive(false);
                gangLightPoolDictionary.Remove(gangDictionary.Keys.ToList()[i]);
                gangDictionary[gangDictionary.Keys.ToList()[i]] = false;
            }
        }
    }

    private void CounterIncrease()
    {
        gangPoolCounter++;
        if (gangPoolCounter >= gangLightPoolSize)
        {
            gangPoolCounter = 0;
        }
    }

    public bool CheckInBoundary(Vector3Int pos)
    {
        Vector3 playerPos = player.transform.position;
        if (pos.x>playerPos.x-width && pos.x < playerPos.x + width && pos.y < playerPos.y + height && pos.y > playerPos.y - height)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
