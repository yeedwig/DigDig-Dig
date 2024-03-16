using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DigManager : MonoBehaviour
{
    public GameObject player;
    public GameObject toolManagerObject;
    public ToolManager toolManager;
    // Start is called before the first frame update
    void Start()
    {
        toolManager = toolManagerObject.GetComponent<ToolManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            Vector2 direction = new Vector2(0, 0);
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                direction = new Vector2(-1, 0);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                direction = new Vector2(1, 0);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                direction = new Vector2(0, -1);
            }

            int layerMask = 1 << LayerMask.NameToLayer("Ground");
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position, direction, 0.6f, layerMask);

            if (hit.collider != null)
            {
                if (Input.GetKey(KeyCode.Q))
                {
                    
                    if (toolManager.curToolSet == 0)
                    {
                        hit.transform.gameObject.GetComponent<Ground>().takeDamage(1.0f); 
                    }
                    else if(toolManager.curToolSet == 1&&direction.x==0)
                    {
                        hit.transform.gameObject.GetComponent<Ground>().takeDamage(5.0f);
                    }
                    
                }

            }

        }
    }
}
