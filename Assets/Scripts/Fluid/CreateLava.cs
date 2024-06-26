using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLava : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FluidManager.instance.lavaBlockDictionary.Add(TilemapManager.instance.lavaTilemap.WorldToCell(this.transform.position), 0);
    }

}
