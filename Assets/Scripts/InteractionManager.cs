using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
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
    private Collider2D elevatorFirst;
    //d
    public bool arrivedUp, arrivedDown;

    //ø§∏Æ∫£¿Ã≈Õ
    private GameObject topElevator;
    private GameObject bottomElevator;


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
                if (elevatorCheck.gameObject.GetComponent<Elevator>().isConnected)
                {
                    if (!isOnElevator)
                    {
                        isOnElevator = true;
                        elevator.SetActive(true);
                        elevator.transform.position = elevatorCheck.transform.position;
                        if (elevatorCheck.gameObject.GetComponent<Elevator>().isTop)
                        {
                            topElevator = elevatorCheck.gameObject;
                            bottomElevator = elevatorCheck.gameObject.GetComponent<Elevator>().pair;
                            topElevator.GetComponent<Elevator>().stoolbc.isTrigger = true;
                            bottomElevator.GetComponent<Elevator>().roofbc.isTrigger = true;
                           // StartCoroutine(MoveElevatorToBottom(topElevator, bottomElevator));

                        }
                        else
                        {
                            bottomElevator = elevatorCheck.gameObject;
                            topElevator = elevatorCheck.gameObject.GetComponent<Elevator>().pair;
                            topElevator.GetComponent<Elevator>().stoolbc.isTrigger = true;
                            bottomElevator.GetComponent<Elevator>().roofbc.isTrigger = true;
                        }
                        /*
                        topElevator.GetComponent<Elevator>().roofbc.isTrigger=true;
                        topElevator.GetComponent<Elevator>().roofbc.isTrigger = true;
                        topElevator.GetComponent<Elevator>().roofbc.isTrigger = true;
                        topElevator.GetComponent<Elevator>().roofbc.isTrigger = true;
                        */

                        elevatorFirst = elevatorCheck;
                        arrivedUp = false;
                        arrivedDown = false;
                        //elevatorFirst.gameObject.GetComponent<Elevator>().stool.SetActive(false);
                        StartCoroutine(MoveElevator(elevatorCheck));
                    }
                }
                
                
            }
        }
        
    }
    /*
    IEnumerator MoveElevatorToBottom(GameObject top, GameObject bottom)
    {

    }
    */

    IEnumerator MoveElevator(Collider2D collider)
    {
        float elevatorSpeed = 0.5f;
        bool speedUp = true;
        if (collider.GetComponent<Elevator>().isTop)
        {
            while (isOnElevator)
            {
                RaycastHit2D ray = Physics2D.Raycast(this.transform.position + new Vector3(0, -0.6f, 0), new Vector2(0, -1), 2.0f, layerMask);
                
                elevatorRB.velocity = Vector2.down * (elevatorSpeed);
                if (speedUp)
                {
                    if (elevatorSpeed < 2.0f)
                    {
                        elevatorSpeed += 0.01f;
                    }
                }
                else
                {
                    if (elevatorSpeed >= 0.5f)
                    {
                       elevatorSpeed -= 0.01f;
                    }
                }
                
                if(ray.collider != null)
                {
                    if (ray.collider.gameObject.tag == "Elevator")
                    {
                        speedUp = false;
                    }
                    
                }

                if (arrivedDown)
                {
                    isOnElevator = false;
                    elevator.SetActive(false);
                    elevatorRB.velocity = Vector2.down * 0.0f;
                    //elevatorFirst.gameObject.GetComponent<Elevator>().stool.SetActive(true);
                }
                

                yield return null;
            }
        }
        else
        {
            while (isOnElevator)
            {
                RaycastHit2D ray = Physics2D.Raycast(this.transform.position + new Vector3(0, 0.6f, 0), new Vector2(0, 1), 0.8f, layerMask);
                elevatorRB.velocity = Vector2.up * (elevatorSpeed);
                if (speedUp)
                {
                    if (elevatorSpeed < 2.0f)
                    {
                        elevatorSpeed += 0.01f;
                    }
                }
                else
                {
                    if (elevatorSpeed >= 0.5f)
                    {
                        elevatorSpeed -= 0.01f;
                    }
                }

                if (ray.collider != null)
                {
                    if (ray.collider.gameObject.tag == "Elevator")
                    {
                        speedUp = false;
                    }
                }

                if (arrivedUp)
                {
                    isOnElevator = false;
                    elevator.SetActive(false);
                    elevatorRB.velocity = Vector2.up * 0.0f;
                    //elevatorFirst.gameObject.GetComponent<Elevator>().stool.SetActive(true);
                }


                yield return null;
            }
        }
        
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(structureCheckPos.transform.position, 0.4f);
        
        
    }
}
