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

    //엘리베이터
    private GameObject topElevator;
    private GameObject bottomElevator;
    [SerializeField] float elevatorMaxSpeed;
    [SerializeField] float elevatorMinSpeed;
    [SerializeField] float elevatorSpeedGap;
    private float elevatorSpeed;
    private RaycastHit2D elevatorRay;
    private RaycastHit2D elevatorSubRay;
    private int elevatorMask;
    private int elevatorSubMask;


    // Start is called before the first frame update
    void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Structure");
        elevatorRB = elevator.GetComponent<Rigidbody2D>();
        isOnRail = false;
        isOnElevator = false;
        elevatorMask = 1 << LayerMask.NameToLayer("Structure");
        elevatorSubMask = 1 << LayerMask.NameToLayer("ElevatorSub");
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D structure = Physics2D.OverlapCircle(structureCheckPos.transform.position, 0.4f, layerMask);
        Collider2D elevatorCheck = Physics2D.OverlapCircle(elevatorCheckPos.transform.position, 0.1f, elevatorMask);

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
                        this.gameObject.transform.position = elevatorCheck.transform.position + new Vector3(0, -0.15f, 0); //엘베 크기 바뀌거나 하면 수정
                        if (elevatorCheck.gameObject.GetComponent<Elevator>().isTop)
                        {
                            topElevator = elevatorCheck.gameObject;
                            bottomElevator = elevatorCheck.gameObject.GetComponent<Elevator>().pair;
                            topElevator.GetComponent<Elevator>().stoolbc.isTrigger = true;
                            bottomElevator.GetComponent<Elevator>().roofbc.isTrigger = true;
                            StartCoroutine(MoveElevatorToBottom(topElevator, bottomElevator));
                        }
                        else
                        {
                            bottomElevator = elevatorCheck.gameObject;
                            topElevator = elevatorCheck.gameObject.GetComponent<Elevator>().pair;
                            topElevator.GetComponent<Elevator>().stoolbc.isTrigger = true;
                            bottomElevator.GetComponent<Elevator>().roofbc.isTrigger = true;
                            StartCoroutine(MoveElevatorToTop(topElevator, bottomElevator));
                        }
                        

                        elevatorFirst = elevatorCheck;
                        arrivedUp = false;
                        arrivedDown = false;
                    }
                }
            }
        }   
    }
    
    IEnumerator MoveElevatorToBottom(GameObject top, GameObject bottom)
    {
        GameObject stool = bottom.transform.GetChild(1).gameObject;
        bool almostArrived = false,arrived=false;
        elevatorSpeed = 0.0f;
        while (!arrived)
        {
            elevatorRay = Physics2D.Raycast(elevator.transform.position+new Vector3(0,-1f,0), new Vector2(0, -1), 0.3f, elevatorMask);
            elevatorSubRay = Physics2D.Raycast(elevator.transform.position+new Vector3(0, -0.5f, 0), new Vector2(0, -1), 0.03f, elevatorSubMask); 
            if (elevatorRay.collider!=null&&elevatorRay.collider.gameObject == bottom)
            {
                almostArrived = true;
            }
            if (elevatorSubRay.collider!=null&&elevatorSubRay.collider.gameObject == stool)
            {
                arrived = true;
            }
            if (!almostArrived)
            {
                if(elevatorSpeed < elevatorMaxSpeed)
                {
                    elevatorSpeed += elevatorSpeedGap;
                }
            }
            else
            {
                if (elevatorSpeed > elevatorMinSpeed)
                {
                    elevatorSpeed -= elevatorSpeedGap;
                }
            }
            if (!arrived)
            {
                elevatorRB.velocity = Vector2.down * (elevatorSpeed);
            }
            else
            {
                isOnElevator = false;
                elevator.SetActive(false);
                top.GetComponent<Elevator>().stoolbc.isTrigger = false;
                bottom.GetComponent<Elevator>().roofbc.isTrigger = false;
                elevatorRB.velocity = Vector2.down * 0.0f;
            }
            yield return null;
        }
        
        
    }

    IEnumerator MoveElevatorToTop(GameObject top, GameObject bottom)
    {
        GameObject roof = top.transform.GetChild(0).gameObject;
        bool almostArrived = false, arrived = false;
        elevatorSpeed = 0.0f;
        while (!arrived)
        {
            elevatorRay = Physics2D.Raycast(elevator.transform.position + new Vector3(0, 1f, 0), new Vector2(0, 1), 0.3f, elevatorMask);
            elevatorSubRay = Physics2D.Raycast(elevator.transform.position + new Vector3(0, 0.5f, 0), new Vector2(0, 1), 0.03f, elevatorSubMask);
            if (elevatorRay.collider != null && elevatorRay.collider.gameObject == top)
            {
                almostArrived = true;
            }
            if (elevatorSubRay.collider != null && elevatorSubRay.collider.gameObject == roof)
            {
                arrived = true;
            }
            if (!almostArrived)
            {
                if (elevatorSpeed < elevatorMaxSpeed)
                {
                    elevatorSpeed += elevatorSpeedGap;
                }
            }
            else
            {
                if (elevatorSpeed > elevatorMinSpeed)
                {
                    elevatorSpeed -= elevatorSpeedGap;
                }
            }
            if (!arrived)
            {
                elevatorRB.velocity = Vector2.up * (elevatorSpeed);
            }
            else
            {
                isOnElevator = false;
                elevator.SetActive(false);
                top.GetComponent<Elevator>().stoolbc.isTrigger = false;
                bottom.GetComponent<Elevator>().roofbc.isTrigger = false;
                elevatorRB.velocity = Vector2.up * 0.0f;
            }
            yield return null;
        }


    }



}
