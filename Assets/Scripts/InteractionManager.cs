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

    //¿¤¸®º£ÀÌÅÍ
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

    [SerializeField] GameObject elevatorCheckRightBottom;
    [SerializeField] GameObject elevatorCheckLeftTop;

    //¿¡µ÷Ã¢ È®ÀÎ
    [SerializeField] GameObject edit;
    private EditController EC;

    [SerializeField] GameObject cart;

    //¿¤¸®º£ÀÌÅÍ ÁÂÇ¥·Î µµÀü


    // Start is called before the first frame update
    void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Structure");
        elevatorRB = elevator.GetComponent<Rigidbody2D>();
        isOnRail = false;
        isOnElevator = false;
        elevatorMask = 1 << LayerMask.NameToLayer("Structure");
        elevatorSubMask = 1 << LayerMask.NameToLayer("ElevatorSub");
        EC = edit.GetComponent<EditController>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D structure = Physics2D.OverlapCircle(structureCheckPos.transform.position, 0.4f, layerMask);
        Collider2D elevatorCheck = Physics2D.OverlapArea(elevatorCheckLeftTop.transform.position, elevatorCheckRightBottom.transform.position, elevatorMask);
        if(!EC.isEditOn)
        {
            if (structure != null)
            {
                if (structure.gameObject.tag == "Rail" && Input.GetKeyDown(KeyCode.F))
                {
                    
                    if (!isOnRail)
                    {
                        cart.SetActive(true);
                        this.gameObject.GetComponent<PlayerManager>().walkSpeed = 5.0f;
                    }
                    else
                    {
                        cart.SetActive(false);
                        this.gameObject.GetComponent<PlayerManager>().walkSpeed = 2.0f;
                    }
                    isOnRail = !isOnRail;
                }
            }
            else
            {
                if (isOnRail)
                {
                    cart.SetActive(false);
                    this.gameObject.GetComponent<PlayerManager>().walkSpeed = 2.0f;
                    isOnRail = false;
                }
            }

            if (elevatorCheck != null)
            {
                if (elevatorCheck.gameObject.tag == "Elevator" && Input.GetKeyDown(KeyCode.F))
                {
                    if (elevatorCheck.gameObject.GetComponent<Elevator>().isConnected)
                    {
                        if (!isOnElevator)
                        {
                            isOnElevator = true;
                            elevator.SetActive(true);
                            elevator.transform.position = elevatorCheck.transform.position;
                            this.gameObject.transform.position = elevatorCheck.transform.position + new Vector3(0, -0.15f, 0); //¿¤º£ Å©±â ¹Ù²î°Å³ª ÇÏ¸é ¼öÁ¤
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
                        }
                    }
                }
            }
        }
         
    }

    IEnumerator MoveElevatorToBottom(GameObject top, GameObject bottom)
    {
        elevatorSpeed = 0.1f;

        while (elevator.transform.position.y>=bottom.transform.position.y)
        {
            if (elevator.transform.position.y - bottom.transform.position.y <= 0.9f) //µµÂø Á÷ÀüÀÏ¶§
            {
                Debug.Log("slowDown");
                if(elevatorSpeed>=elevatorMinSpeed) elevatorSpeed -= elevatorSpeedGap;
            }
            else //µµÂø Á÷ÀüÀÌ ¾Æ´Ò¶§
            {
                if(elevatorSpeed < elevatorMaxSpeed) elevatorSpeed += elevatorSpeedGap;
            }
            elevatorRB.velocity = Vector2.down * (elevatorSpeed);

            yield return null;
        }

        isOnElevator = false;
        elevator.SetActive(false);
        top.GetComponent<Elevator>().stoolbc.isTrigger = false;
        bottom.GetComponent<Elevator>().roofbc.isTrigger = false;
        elevatorRB.velocity = Vector2.down * 0.0f;
    }
    IEnumerator MoveElevatorToTop(GameObject top, GameObject bottom)
    {
        elevatorSpeed = 0.1f;

        while (elevator.transform.position.y <= top.transform.position.y)
        {
            if (top.transform.position.y - elevator.transform.position.y <= 0.9f) //µµÂø Á÷ÀüÀÏ¶§
            {
                if (elevatorSpeed >= elevatorMinSpeed) elevatorSpeed -= elevatorSpeedGap;
            }
            else //µµÂø Á÷ÀüÀÌ ¾Æ´Ò¶§
            {
                if (elevatorSpeed < elevatorMaxSpeed) elevatorSpeed += elevatorSpeedGap;
            }
            elevatorRB.velocity = Vector2.up * (elevatorSpeed);

            yield return null;
        }

        isOnElevator = false;
        elevator.SetActive(false);
        top.GetComponent<Elevator>().stoolbc.isTrigger = false;
        bottom.GetComponent<Elevator>().roofbc.isTrigger = false;
        elevatorRB.velocity = Vector2.up * 0.0f;
    }
}
