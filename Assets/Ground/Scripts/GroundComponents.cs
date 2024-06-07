using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundComponents : MonoBehaviour
{
    public static GroundComponents instance = null;
    public GroundSO[] groundSO;
    void Awake()
    {
        instance = this;
    }

    
    
}
