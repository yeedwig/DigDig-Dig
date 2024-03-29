using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    private Rigidbody2D rb;
    public float force;
    private bool starts;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        starts = false;
        force = 10.0f;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (starts)
        {
            rb.velocity = Vector2.up * 3.0f;
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            starts = true;
        }
    }
}
