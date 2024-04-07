using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public bool isTop;
    public GameObject stool;
    private LineRenderer lr;
    private bool isDrawingLine = false;
    [SerializeField] GameObject edit;
    private EditController editController;
    [SerializeField] float test = 10;
    public bool isConnected = false;
    

    private void Start()
    {
        stool = transform.GetChild(0).gameObject;
        lr = GetComponent<LineRenderer>();
        editController = GameObject.Find("Edit").GetComponent<EditController>();
        lr.startWidth = 0.02f;
        lr.endWidth = 0.02f;
    }

    private void Update()
    {
        if (editController.isEditOn && !isConnected && (editController.itemCursorIndex == 4 || editController.itemCursorIndex == 5))
        {
            lr.positionCount = 2;
            
            if (isTop)
            {
                lr.SetPosition(0, this.transform.position + new Vector3(-0.2f,0,0));
                lr.SetPosition(1, this.transform.position + new Vector3(-0.2f, -test, 0));
            }
            else
            {
                lr.SetPosition(0, this.transform.position + new Vector3(0.2f, 0, 0));
                lr.SetPosition(1, this.transform.position + new Vector3(0.2f, test, 0));
            }
        }
        else
        {
            lr.positionCount = 0;
        }
    }
}
