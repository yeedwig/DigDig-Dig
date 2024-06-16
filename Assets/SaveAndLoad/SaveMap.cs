using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;
using System.Linq;

public class SaveMap
{
    private static readonly string SAVE_FOLDER = Application.dataPath + "/SaveAndLoad/";
    public class MapObjects
    {
        public List<Vector3Int> diggedKey;
        public List<Vector3Int> gangKey;
        public List<Vector3Int> railKey;
        public List<Vector3Int> ladderKey;
        public List<bool> ladderIsLeft;
        public List<Vector3Int> passageKey;
        public List<Vector3> topKey;
        public List<Vector3> topValue;
        public List<Vector3> botKey;
        public List<Vector3> botValue;
    }

    public static void saveMap(Dictionary<GameObject,GameObject>topDic, Dictionary<GameObject, GameObject> botDic,TileBase leftLadderTile, TileBase rightLadderTile)
    {
        MapObjects mapObject = new MapObjects
        {
            diggedKey = new List<Vector3Int>(),
            gangKey = new List<Vector3Int>(),
            railKey = new List<Vector3Int>(),
            ladderKey = new List<Vector3Int>(),
            ladderIsLeft = new List<bool>(),
            passageKey = new List<Vector3Int>(),
            topKey = new List<Vector3>(),
            topValue = new List<Vector3>(),
            botKey = new List<Vector3>(),
            botValue = new List<Vector3>(),
        };
        foreach (Vector3Int key in GangController.instance.gangDictionary.Keys)
        {
            mapObject.gangKey.Add(key);
        }
        foreach (KeyValuePair<Vector3Int, GameObject> pair in GroundDictionary.instance.groundDictionary)
        {
            Ground ground = pair.Value.GetComponent<Ground>();
            if (!ground.bc.enabled && !ground.isBlank)
            {
                mapObject.diggedKey.Add(pair.Key);
            }
            if (TilemapManager.instance.railTilemap.GetTile(pair.Key) != null)
            {
                mapObject.railKey.Add(pair.Key);
            }
            if (TilemapManager.instance.elevatorPassageTilemap.GetTile(pair.Key) != null)
            {
                mapObject.passageKey.Add(pair.Key);
            }

            if (TilemapManager.instance.ladderTilemap.GetTile(pair.Key) == leftLadderTile)
            {
                mapObject.ladderKey.Add(pair.Key);
                mapObject.ladderIsLeft.Add(true);
            }
            else if (TilemapManager.instance.ladderTilemap.GetTile(pair.Key) == rightLadderTile)
            {
                mapObject.ladderKey.Add(pair.Key);
                mapObject.ladderIsLeft.Add(false);
            }
        }
        foreach (KeyValuePair<GameObject, GameObject> pair in topDic)
        {
            mapObject.topKey.Add(pair.Key.transform.position);
            if (pair.Value != null)
            {
                mapObject.topValue.Add(pair.Value.transform.position);
            }
            else
            {
                mapObject.topValue.Add(new Vector3(-1, 1, 0));
            }
        }
        foreach (KeyValuePair<GameObject, GameObject> pair in botDic)
        {
            mapObject.botKey.Add(pair.Key.transform.position);
            if (pair.Value != null)
            {
                mapObject.botValue.Add(pair.Value.transform.position);
            }
            else
            {
                mapObject.botValue.Add(new Vector3(-1, 1, 0));
            }
        }

        string json = JsonUtility.ToJson(mapObject);
        File.WriteAllText(SAVE_FOLDER + "/MapSave.txt", json);
    }

    public static void loadMap(Dictionary<GameObject, GameObject> topDic, Dictionary<GameObject, GameObject> botDic, TileBase railTile, TileBase elevatorPassageTile, TileBase leftLadderTile,
        TileBase rightLadderTile, GameObject elevatorTop, GameObject elevatorBot)
    {
        if (File.Exists(SAVE_FOLDER + "/MapSave.txt"))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + "/MapSave.txt");
            MapObjects mapObject = JsonUtility.FromJson<MapObjects>(saveString);
            foreach (Vector3Int key in mapObject.diggedKey)
            {
                Ground ground = GroundDictionary.instance.groundDictionary[key].GetComponent<Ground>();
                ground.currentHealth = -100.0f;
                ground.bc.enabled = false;
                ground.ChangeSpriteByCurrentHealth();
            }
            foreach (Vector3Int key in mapObject.gangKey)
            {
                GangController.instance.CreateGang(key);

            }
            foreach (Vector3Int key in mapObject.railKey)
            {
                TilemapManager.instance.railTilemap.SetTile(key, railTile);

            }
            foreach (Vector3Int key in mapObject.passageKey)
            {
                TilemapManager.instance.elevatorPassageTilemap.SetTile(key, elevatorPassageTile);

            }
            Dictionary<Vector3Int, bool> ladderDic = mapObject.ladderKey.Zip(mapObject.ladderIsLeft, (k, v) => new { k, v }).ToDictionary(a => a.k, a => a.v);
            foreach (KeyValuePair<Vector3Int, bool> pair in ladderDic)
            {
                if (pair.Value)
                {
                    TilemapManager.instance.ladderTilemap.SetTile(pair.Key, leftLadderTile);
                }
                else
                {
                    TilemapManager.instance.ladderTilemap.SetTile(pair.Key, rightLadderTile);
                }
            }
            Dictionary<Vector3, Vector3> topLoadDic = mapObject.topKey.Zip(mapObject.topValue, (k, v) => new { k, v }).ToDictionary(a => a.k, a => a.v);
            foreach (KeyValuePair<Vector3, Vector3> pair in topLoadDic)
            {
                GameObject top = GameObject.Instantiate(elevatorTop);
                top.transform.position = pair.Key;
                if (Vector3.Distance(new Vector3(-1, 1, 0), pair.Value) >= 0.1f) //Â¦ÀÌ ÀÖ´Ù
                {
                    GameObject bot = GameObject.Instantiate(elevatorBot);
                    bot.transform.position = pair.Value;
                    top.GetComponent<Elevator>().isConnected = true;
                    top.GetComponent<Elevator>().pair = bot;
                    bot.GetComponent<Elevator>().isConnected = true;
                    bot.GetComponent<Elevator>().pair = top;
                    topDic.Add(top, bot);
                    botDic.Add(bot, top);
                }
                else
                {
                    topDic.Add(top, null);
                }
            }
            Dictionary<Vector3, Vector3> botLoadDic = mapObject.botKey.Zip(mapObject.botValue, (k, v) => new { k, v }).ToDictionary(a => a.k, a => a.v);
            foreach (KeyValuePair<Vector3, Vector3> pair in botLoadDic)
            {
                if (Vector3.Distance(new Vector3(-1, 1, 0), pair.Value) <= 0.1f) //Â¦ÀÌ ¾ø´Ù
                {
                    GameObject bot = GameObject.Instantiate(elevatorBot);
                    bot.transform.position = pair.Key;
                    botDic.Add(bot, null);
                }
            }

        }
    }

}
