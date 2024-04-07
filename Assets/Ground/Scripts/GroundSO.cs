using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GroundSO : ScriptableObject
{
    public int Lv;
    public Sprite[] groundSprites;
    public int groundMaxPerLv;
    public float groundHealth;
}
