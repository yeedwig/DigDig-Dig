using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleDoorCheck : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] MoleDoor moleDoor;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        moleDoor.doorOpened = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && gameManager.hasMoleId == true)
        {
            moleDoor.doorOpened=true;
            Debug.Log("Open");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && gameManager.hasMoleId == true)
        {
            moleDoor.doorOpened = false;
            Debug.Log("close");
        }
    }
    // Update is called once per frame
    /*
    void Update()
    {

        Collider2D playerChecked = Physics2D.OverlapCircle(transform.position, 1.0f, LayerMask.GetMask("Player"));

        if (playerChecked != null && gameManager.hasMoleId == true)
        {
            moleDoor.doorOpened = true;
            Debug.Log("Open");

        }

        if (playerChecked == null)
        {
            moleDoor.doorOpened = false;
            Debug.Log("Close");
        }
    }*/
}
