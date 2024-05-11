using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Renderer>().sortingLayerName = "Player";
        this.GetComponent<Renderer>().sortingOrder = 3;
    }

    // Update is called once per frame
    
}
