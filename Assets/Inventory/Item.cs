using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string Name;
    public string Description;
    public int  itemType;
    public int itemId;
    public int resourceId;
    public int efficiency;

    public int price;
    public float damage;
    public int durability;
    //public ActionType actionType;
    public bool isTool;
    public bool isResource;
    public bool isStructure;
    public bool isKey;

    public bool stackable = true;
    public int maxStack;

    public Sprite image;
}


