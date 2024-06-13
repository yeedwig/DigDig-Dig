using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGas : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FluidManager.instance.gasBlockDictionary.Add(TilemapManager.instance.gasTilemap.WorldToCell(this.transform.position), 0);
    }

    
}
