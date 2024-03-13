using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileTest : MonoBehaviour
{
    public GameObject player;
    public Tilemap tilemap;
    public TileBase tile1,tile2;
    private Dictionary<Vector3Int,float> tiles = new Dictionary<Vector3Int,float>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E)|| Input.GetKey(KeyCode.Q))
        {
            Vector2 direction = new Vector2(0,0);
            if(Input.GetKey(KeyCode.LeftArrow))
            {
                direction = new Vector2(-1, 0);
            }
            else if(Input.GetKey(KeyCode.RightArrow))
            {
                direction = new Vector2(1, 0);
            }
            else if(Input.GetKey(KeyCode.DownArrow))
            {
                direction = new Vector2(0, -1);
            }
            
            int layerMask = 1 << LayerMask.NameToLayer("Ground");
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position,direction, 0.6f,layerMask);

            if (hit.collider != null)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    hit.transform.gameObject.GetComponent<Ground>().takeDamage(0.3f);
                }
                else if (Input.GetKey(KeyCode.Q)&&direction.x==0)
                {
                    hit.transform.gameObject.GetComponent<Ground>().takeDamage(5.0f);
                }
                
            }
            
        }
    }
}
