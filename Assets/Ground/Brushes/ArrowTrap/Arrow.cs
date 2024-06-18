using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private bool rightShoot;
    private float dir;

    //private Animator anim;
    private BoxCollider2D bc;
    private Rigidbody2D rb;

    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animator>();
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        if(rightShoot)
        {
            dir = 1;
        }
        else
        {
            dir = -1;
        }
        Invoke("DestroyProjectile", lifeTime);
    }

    void Update()
    {
        rb.velocity = new Vector2( dir * speed, 0);
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.GetComponent<PlayerManager>() != null)
        {
            other.GetComponent<Health>().takeDamage(damage);
            DestroyProjectile();
            //anim.SetTrigger("explode");
        }
    }
}
