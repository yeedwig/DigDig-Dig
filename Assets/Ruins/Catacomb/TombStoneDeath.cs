using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TombStoneDeath : MonoBehaviour
{
    private NPC npc;
    private int index;
    private PlayerManager player;
    private void Start()
    {
        npc = this.GetComponent<NPC>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();

    }

    private void Update()
    {
        if(npc.index == 3)
        {
            player.Dead = true;
        }
    }


}
