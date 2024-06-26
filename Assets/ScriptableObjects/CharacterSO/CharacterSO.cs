using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterSO : ScriptableObject
{
    public string Name;
    public int ID;
    public string Species;
    public int type;

    public float moveSpeed;
    //±âÅ¸ Æ¯Â¡µé

    public Sprite[] IdleSkins;
    public Sprite[] DiggingSkins;
    public Sprite[] WalkingSkins;
    public Sprite[] DrillingSkins;
    public Sprite[] ClimbingSkins;

}

