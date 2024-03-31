using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] GameObject structureCheckPos;
    [SerializeField] GameObject elevatorCheckPos;
    private int layerMask;
    private bool isOnRail;
    [SerializeField] GameObject elevator;
    private Rigidbody2D elevatorRB;
    private bool isOnElevator;
    // Start is called before the first frame update
    void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Structure");
        elevatorRB = elevator.GetComponent<Rigidbody2D>();
        isOnRail = false;
        isOnElevator = false;
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D structure = Physics2D.OverlapCircle(structureCheckPos.transform.position, 0.4f, layerMask);
        Collider2D elevatorCheck = Physics2D.OverlapCircle(elevatorCheckPos.transform.position, 0.1f, layerMask);
        if (structure != null)
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
        }
        else
        {
            if(isOnRail)
            {
                this.gameObject.GetComponent<PlayerManager>().walkSpeed = 3.0f;
                isOnRail = false;
            }
        }

        if(elevatorCheck != null)
        {
            if(elevatorCheck.gameObject.tag == "Elevator" && Input.GetKeyDown(KeyCode.F))
            {
                if (!isOnElevator)
                {
                    isOnElevator = true;
                    elevator.SetActive(true);
                    elevator.transform.position = elevatorCheck.transform.position;
                    this.transform.position = elevator.transform.position;
                    StartCoroutine(MoveElevator(elevatorCheck));
                }
                
            }
        }
        
    }
    IEnumerator MoveElevator(Collider2D elevator)
    {
        float elevatorSpeed = 0.5f;
        bool speedUp = true;
        if (elevator.GetComponent<Elevator>().isTop)
        {
            while (isOnElevator)
            {
                elevatorRB.velocity = Vector2.down * (elevatorSpeed);
                if (speedUp)
                {
                    if (elevatorSpeed < 2.0f)
                    {
                        elevatorSpeed += 0.5f;
                    }
                }
                else
                {
                    if (elevatorSpeed >= 0.4f)
                    {
                        elevatorSpeed -= 0.5f;
                    }
                }
                RaycastHit2D ray = Physics2D.Raycast(this.transform.position, new Vector2(0, -1), 0.7f, layerMask);
                if(ray.collider != null)
                {
                    if (ray.collider.gameObject.tag == "Elevator")
                    {
                        Debug.Log("elevator");
                    }
                }

                yield return new WaitForSeconds(1.0f);
            }
        }
        else
        {
            while (isOnElevator)
            {

                elevatorRB.velocity = Vector2.up * (elevatorSpeed);
                if (elevatorSpeed < 2.0f)
                {
                    elevatorSpeed += 0.5f;
                }
                yield return new WaitForSeconds(1.0f);
            }
        }
        
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(structureCheckPos.transform.position, 0.4f);
        
        
    }
}
