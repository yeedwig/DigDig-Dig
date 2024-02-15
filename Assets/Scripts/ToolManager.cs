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
    private bool isWalking;

    public int skinNr;

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
        isWalking = Player.GetComponent<PlayerMovement>().isWalking;
        UpdateAnimation();
    }

    void LateUpdate()
    {
        if(sp.sprite.name.Contains("MainShovel"))
        {
            string spriteName = sp.sprite.name;
            spriteName = spriteName.Replace("MainShovel_","");
            int spriteNr = int.Parse(spriteName);

            sp.sprite = shovelSkins[skinNr].sprites[spriteNr];
        }

    }


    private void UpdateAnimation()
    {
        anim.SetBool("isDrilling", isDrilling);
        anim.SetBool("isDigging", isDigging);
        anim.SetBool("isWalking", isWalking);
    }


    [System.Serializable] // inspetor에서 보이게 하는 기능
    public struct Skins
    {
        public Sprite[] sprites;
    }
}
