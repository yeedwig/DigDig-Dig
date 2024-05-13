using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndLoadTest : MonoBehaviour
{
    // Start is called before the first frame update
    public int testInt;
    void Start()
    {
        testInt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            testInt++;
        }
    }
}
