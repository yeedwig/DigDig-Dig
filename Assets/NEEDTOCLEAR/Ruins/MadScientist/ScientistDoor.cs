using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistDoor : MonoBehaviour
{
    private bool DoorOpen = false;
    [SerializeField] private ScientistButton[] Buttons;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D playerChecked = Physics2D.OverlapCircle(transform.position, 2.0f, LayerMask.GetMask("Player"));

        if (playerChecked != null && Input.GetKeyDown(KeyCode.F) && DoorOpen == false)
        {
            if (checkButtonsOn())///비번이 맞으면
            {
                DoorOpen = true;
                OpenDoor();
            }

        }
    }

    private bool checkButtonsOn()
    {
        for(int i = 0; i < Buttons.Length; i++)
        {
            if (Buttons[i].isOn == false)
            {
                return false; 
            }
        }

        return true;
    }

    public void OpenDoor()
    {
        this.gameObject.SetActive(false);
    }
}
