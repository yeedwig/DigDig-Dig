using System.Collections;
using System.Collections.Generic;
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
                    //StartCoroutine(MoveElevator());
                }
                
            }
        }
        
    }
    IEnumerator MoveElevator()
    {
        float elevatorSpeed = 0.0f;
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            Debug.Log(elevatorSpeed);
            elevatorRB.velocity = Vector2.down * (elevatorSpeed);
            if (elevatorSpeed < 3.0f)
            {
                elevatorSpeed += 0.5f;
            }
        }
        yield return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(structureCheckPos.transform.position, 0.4f);
        
        
    }
}
