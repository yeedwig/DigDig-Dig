using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] public bool triggered;
    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float arrowShootTime;

    public float spotRange;

    private Transform player;
    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (Vector2.Distance(player.position, this.gameObject.transform.position) <= spotRange)
        {
            shootArrow();
        }
    }


    private IEnumerator shootArrow()
    {
        yield return new WaitForSeconds(arrowShootTime);
        Instantiate(arrow, firePoint.position, firePoint.rotation);
    }


}
