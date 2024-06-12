using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WorkerAnt : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    private Vector2 target;
    private Vector2 newPos;
    private bool isFacingLeft = true;
    [SerializeField] private float walkTime;
    private float walkTimer;

    public float honeySpawnTime;
    public float speed = 1.0f;

    public bool carryingHoney;
    public Transform honeyRespawnPos;
    public GameObject honey;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        carryingHoney = false;
        walkTimer = 0.0f;
    }
    private void Update()
    {
        walkTimer += Time.deltaTime;
        if(walkTimer > walkTime)
        {
            
            target = new Vector2(this.transform.position.x + Random.Range(-1.5f, 1.5f), this.transform.position.y);
            if (transform.position.x > target.x && !isFacingLeft)
            {
                transform.Rotate(0f, 180f, 0f);
                isFacingLeft = true;
            }
            else if (transform.position.x < target.x && isFacingLeft)
            {
                transform.Rotate(0f, 180f, 0f);
                isFacingLeft = false;
            }
            anim.SetTrigger("Walk");
            walkTimer = 0;
        }


        
        newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

    }


}
