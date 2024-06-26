using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public bool isTop;
    public BoxCollider2D roofbc;
    public BoxCollider2D stoolbc;
    private LineRenderer lr;
    private bool isDrawingLine = false;
    [SerializeField] GameObject edit;
    private EditController editController;
    [SerializeField] float test = 10;
    public bool isConnected = false;
    public GameObject pair;
    int layermask;
    

    private void Start()
    {
        roofbc = transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();
        stoolbc = transform.GetChild(1).gameObject.GetComponent<BoxCollider2D>();
        lr = GetComponent<LineRenderer>();
        editController = GameObject.Find("Edit").GetComponent<EditController>();
        lr.startWidth = 0.02f;
        lr.endWidth = 0.02f;
        layermask = 1 << LayerMask.NameToLayer("Ground");
    }

    private void Update()
    {
        if (editController.isEditOn && !isConnected && (editController.itemCursorIndex == 4 || editController.itemCursorIndex == 5))
        {
            lr.positionCount = 2;
            
            if (isTop)
            {
                RaycastHit2D hit = Physics2D.Raycast(this.transform.position, new Vector2(0, -1), GameManager.instance.ElevatorPassageNum + 1,layermask);
                lr.SetPosition(0, this.transform.position + new Vector3(-0.2f,0,0));
                if(hit.collider != null)
                {
                    lr.SetPosition(1, this.transform.position + new Vector3(-0.2f, -hit.distance+0.5f, 0));
                }
                else
                {
                    lr.SetPosition(1, this.transform.position + new Vector3(-0.2f, -GameManager.instance.ElevatorPassageNum - 1, 0));
                }
                
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(this.transform.position, new Vector2(0, 1), GameManager.instance.ElevatorPassageNum + 1, layermask);
                lr.SetPosition(0, this.transform.position + new Vector3(0.2f, 0, 0));
                if (hit.collider != null)
                {
                    lr.SetPosition(1, this.transform.position + new Vector3(-0.2f, hit.distance - 0.5f, 0));
                }
                else
                {
                    lr.SetPosition(1, this.transform.position + new Vector3(0.2f, GameManager.instance.ElevatorPassageNum + 1, 0));
                }
            }
        }
        else
        {
            lr.positionCount = 0;
        }
    }
}
