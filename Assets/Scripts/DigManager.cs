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

    public AudioClip[] diggingSound;
    public AudioClip[] drillingSound;

    private float digDirX;
    private float digDirY;

    // 데미지 넣기 위함
    public bool canDamage = false;


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
        if (Input.GetKey(KeyCode.Q) && (playerManager.isDigging || playerManager.isDrilling))
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
                int curSelectSlot = toolManager.curSelectedSlot;
               float damage = toolManager.curToolDamage;
               int toolId = toolManager.curToolId;
                if (Input.GetKey(KeyCode.Q))
                {
                    if (toolManager.curToolType == 0 && canDamage) //삽
                    {
                        hit.transform.gameObject.GetComponent<Ground>().takeDamage(damage);
                        canDamage = false;
                        //뭔가 여기서 땅이 삽 내구도를 얼마나 닳게 하는지 가져왓음 좋겠음
                        toolManager.useItem(curSelectSlot);
                    }
                    else if(toolManager.curToolType == 1&&direction.x==0 && canDamage) //드릴
                    {
                        hit.transform.gameObject.GetComponent<Ground>().takeDamage(damage);
                        canDamage = false;
                        toolManager.useItem(curSelectSlot);
                    }
                }
            }
        }
    }

    public void DigDoDamage()
    {
        if(toolManager.curToolType == 0)
        {
            SoundFXManager.instance.PlaySoundFXClip(diggingSound, transform, 0.5f);
        }
        else if(toolManager.curToolType == 1)
        {
            SoundFXManager.instance.PlaySoundFXClip(drillingSound, transform, 0.5f);
        }
        canDamage = true;
    }
}
