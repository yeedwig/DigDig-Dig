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
    public float shootDir;

    private Transform player;
    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, new Vector2(shootDir, 0) * spotRange, Color.red, 0);
        RaycastHit2D hit = Physics2D.Raycast(new Vector2 (transform.position.x, transform.position.y - 0.3f), new Vector2(shootDir, 0), spotRange, LayerMask.GetMask("Player"));

        if (hit.collider != null)
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
