using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSprite : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    private SpriteRenderer sp;
    private Animator anim;


    public int curToolSet; //0이면 삽, 1이면 드릴, 2이면 TNT, 3이면 Radar
    private bool ToolisDrilling;
    private bool ToolisDigging;
    private bool ToolisWalking;


    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ToolisDigging = Player.GetComponent<PlayerManager>().isDigging;
        ToolisDrilling = Player.GetComponent<PlayerManager>().isDrilling;
        ToolisWalking = Player.GetComponent<PlayerManager>().isWalking;
        UpdateAnimation();

    }
    private void UpdateAnimation()
    {
        anim.SetBool("isDrilling", ToolisDrilling);
        anim.SetBool("isDigging", ToolisDigging);
        anim.SetBool("isWalking", ToolisWalking);
    }
}
