using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Resource : ScriptableObject
{
    public string Name;
    public string Description;
    public int resourceId;

    public int price;
}
