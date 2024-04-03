using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorCollision : MonoBehaviour
{
    [SerializeField] GameObject player;
    private InteractionManager interactionManager;
    // Start is called before the first frame update
    void Start()
    {
        interactionManager = player.GetComponent<InteractionManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null)
        {
            if(collision.gameObject.name == "Stool")
            {
                interactionManager.arrivedDown = true;
            }
            if(collision.gameObject.name == "Roof")
            {
                interactionManager.arrivedUp = true;
            }
        }
    }
}
