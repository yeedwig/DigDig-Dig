using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] public bool triggered;
    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float arrowShootTime;
    [SerializeField] private float arrowShootTimer;

    public float spotRange;

    private Transform player;
    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (Vector2.Distance(player.position, this.gameObject.transform.position) <= spotRange)
        {
            arrowShootTimer += Time.deltaTime;
            if(arrowShootTimer > arrowShootTime)
            {
                shootArrow();
                arrowShootTimer = 0;
            }
        }
    }


    private void shootArrow()
    {
        Instantiate(arrow, firePoint.position, firePoint.rotation);
    }


}
