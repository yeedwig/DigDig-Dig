using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderLayerChanger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Renderer>().sortingLayerName = "Fluid";
        this.GetComponent<Renderer>().sortingOrder = 0;
    }

    
}
