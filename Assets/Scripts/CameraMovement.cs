using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject target;
    public float moveSpeed;
    private Vector3 targetPosition;

    public BoxCollider2D bound;
    private Vector3 minBound;
    private Vector3 maxBound;

    private float halfWidth;
    private float halfHeight;

    private Camera theCamera;

    //target ∞·¡§
    private EditController editController;
    [SerializeField] GameObject player;
    [SerializeField] GameObject cursor;


    void Start()
    {
        theCamera = GetComponent<Camera>();
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
        halfHeight = theCamera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
        editController = GameObject.Find("Edit").GetComponent<EditController>();
    }
    void Update()
    {
        CheckTarget();
        if(target.gameObject != null)
        {
            targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed*Time.deltaTime);

            float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
            float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

            this.transform.position =  new Vector3(clampedX, clampedY, this.transform.position.z);
        }    
    }   

    public void SetBound(BoxCollider2D newBound)
    {
        bound = newBound;
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
    }

    private void CheckTarget()
    {
        if (editController.isEditOn)
        {
            target = cursor;
        }
        else
        {
            target = player;
        }
    }
}
