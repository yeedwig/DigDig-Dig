using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    private SpriteRenderer sp;
    private Animator anim;
    
    private bool isDrilling;
    private bool isDigging;

    public Skins[] shovelSkins;
    public Skins[] drillSkins;


    public 
    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isDigging = Player.GetComponent<PlayerMovement>().isDigging;
        isDrilling = Player.GetComponent<PlayerMovement>().isDrilling;
        UpdateAnimation();
    }


    private void UpdateAnimation()
    {
        anim.SetBool("isDrilling", isDrilling);
        anim.SetBool("isDigging", isDigging);
    }


    [System.Serializable] // inspetor에서 보이게 하는 기능
    public struct Skins
    {
        public Sprite[] sprites;
    }
}
