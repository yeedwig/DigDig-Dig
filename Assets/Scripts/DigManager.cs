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

    public Item curItem;
    public PlayerManager playerManager;

    private float digDirX;
    private float digDirY;

    // Start is called before the first frame update
    void Start()
    {
        toolManager = toolManagerObject.GetComponent<ToolManager>();
        playerManager = player.GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        digDirX = Input.GetAxisRaw("Horizontal");
        digDirY = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(KeyCode.Q) && !playerManager.isWalking && (playerManager.isDigging || playerManager.isDrilling))
        {
            //여기서 Q를 먼저 받아와서 문제가 생기는 듯함 그래서 Q를 누르고 방향키를 누르면 애니메이션이 그대로 나오는데 반대인경우 안나옴
            Vector2 direction = new Vector2(0, 0);
            if (digDirX < 0 && digDirY == 0)//Input.GetKey(KeyCode.LeftArrow))
            {
                direction = new Vector2(-1, 0);
            }
            else if (digDirX > 0 && digDirY == 0)//Input.GetKey(KeyCode.RightArrow))
            {
                direction = new Vector2(1, 0);
            }
            else if (digDirY < 0)//Input.GetKey(KeyCode.DownArrow))
            {
                direction = new Vector2(0, -1);
            }

            int layerMask = 1 << LayerMask.NameToLayer("Ground");
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position, direction, 1f, layerMask);

            if (hit.collider != null)
            {
                curItem = toolManager.curItem;
                
               float damage = toolManager.curToolDamage;
               int toolId = toolManager.curToolId;
                if (Input.GetKey(KeyCode.Q))
                {
                    if (toolManager.curToolType == 0)
                    {
                        hit.transform.gameObject.GetComponent<Ground>().takeDamage(damage);
                        //toolManager.curToolInvenItem.Durability -= 10;
                    }
                    else if(toolManager.curToolType == 1&&direction.x==0)
                    {
                        hit.transform.gameObject.GetComponent<Ground>().takeDamage(damage);
                    }
                }
            }
        }
    }
}
