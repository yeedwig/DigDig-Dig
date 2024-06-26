using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoulderPush : MonoBehaviour
{
    public GameObject Boulder;
    public bool rollLeft;
    public float pushForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (rollLeft)
            {
                Boulder.GetComponent<Rigidbody2D>().velocity = Vector2.left * pushForce;
            }
            else
            {
                Boulder.GetComponent<Rigidbody2D>().velocity = Vector2.right * pushForce;
            }

        }
    }
}
