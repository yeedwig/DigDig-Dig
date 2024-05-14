using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidInteraction : MonoBehaviour
{
    int fluidMask;
    [SerializeField] GameObject leftTop, rightBottom;
    // Start is called before the first frame update
    void Start()
    {
        fluidMask = LayerMask.GetMask("Default");
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D fluidCollider = Physics2D.OverlapArea(leftTop.transform.position,rightBottom.transform.position,fluidMask);
        if(fluidCollider != null )
        {
            Debug.Log(fluidCollider.gameObject.name);
        }
       
    }

    
}
