using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public float spikeDamage;
    public float knockBackForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Health>().takeDamage(spikeDamage);
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.up * knockBackForce;//AddForce(Vector2.up*knockBackForce, ForceMode2D.Impulse);
        }
    }
    
}
