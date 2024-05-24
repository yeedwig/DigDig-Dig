using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleDoor : MonoBehaviour
{
    private GameManager gameManager;
    private bool doorOpened;
    private Animator anim;
    [SerializeField] private GameObject moleDoor;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        doorOpened = false;
        anim = moleDoor.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
        Collider2D playerChecked = Physics2D.OverlapCircle(transform.position, 1.0f, LayerMask.GetMask("Player"));

        if (playerChecked != null && gameManager.hasMoleId == true)
        {
            doorOpened = true;
            Debug.Log("Open");
            
        }

        if(playerChecked == null)
        {
            doorOpened = false;
            Debug.Log("Close");
        }
        

        anim.SetBool("Open", doorOpened);
    }

    void OpenCloseDoor()
    {
        moleDoor.SetActive(!doorOpened);
    }
}
