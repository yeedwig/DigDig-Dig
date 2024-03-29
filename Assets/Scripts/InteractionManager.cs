using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] GameObject structureCheckPos;
    private int layerMask;
    private bool isOnRail;
    [SerializeField] GameObject elevator;
    // Start is called before the first frame update
    void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Structure");
        isOnRail = false;
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D structure = Physics2D.OverlapCircle(structureCheckPos.transform.position, 0.4f, layerMask);
        if(structure != null)
        {
            if (structure.gameObject.tag == "Rail" && Input.GetKeyDown(KeyCode.F))
            {
                if (!isOnRail)
                {
                    this.gameObject.GetComponent<PlayerManager>().walkSpeed = 15.0f;
                }
                else
                {
                    this.gameObject.GetComponent<PlayerManager>().walkSpeed = 3.0f;
                }
                isOnRail = !isOnRail;
            }
            if(structure.gameObject.tag == "Elevator" && Input.GetKeyDown(KeyCode.F))
            {
                elevator.transform.position = structure.transform.position;
            }
        }
        else
        {
            if(isOnRail)
            {
                this.gameObject.GetComponent<PlayerManager>().walkSpeed = 3.0f;
                isOnRail = false;
            }
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(structureCheckPos.transform.position, 0.4f);
        
        
    }
}
