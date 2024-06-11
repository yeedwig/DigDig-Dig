using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWater : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FluidManager.instance.waterBlockDictionary.Add(TilemapManager.instance.waterTilemap.WorldToCell(this.transform.position), 4);
    }

    // Update is called once per frame
    
}
