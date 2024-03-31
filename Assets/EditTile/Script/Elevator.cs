using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public bool isTop;
    public GameObject stool;

    private void Start()
    {
        stool = transform.GetChild(0).gameObject;
    }

}
