using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtlantisWaterCheck : MonoBehaviour
{
    public bool waterTriggered = false;

    // Update is called once per frame
    void Update()
    {
        Collider2D waterrChecked = Physics2D.OverlapCircle(transform.position, 1.0f, LayerMask.GetMask("Water"));
        if(waterrChecked != null )
        {
            waterTriggered = true;
        }
    }
}
