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

    // ������ �ֱ� ����
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
            //���⼭ Q�� ���� �޾ƿͼ� ������ ����� ���� �׷��� Q�� ������ ����Ű�� ������ �ִϸ��̼��� �״�� �����µ� �ݴ��ΰ�� �ȳ���
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
                    if (toolManager.curToolType == 0 && canDamage) //��
                    {
                        hit.transform.gameObject.GetComponent<Ground>().takeDamage(damage);
                        canDamage = false;
                        //���� ���⼭ ���� �� �������� �󸶳� ��� �ϴ��� �������� ������
                        toolManager.useItem(curSelectSlot);
                    }
                    else if(toolManager.curToolType == 1&&direction.x==0 && canDamage) //�帱
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
