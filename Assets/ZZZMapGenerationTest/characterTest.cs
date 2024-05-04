using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector3(-5,0,0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector3(5,0,0);
        }
    }
}
