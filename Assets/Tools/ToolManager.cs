using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    private SpriteRenderer sp;
    private Animator anim;
    

    public int curToolSet; //0이면 삽, 1이면 드릴, 2이면 TNT, 3이면 Radar
    private bool ToolisDrilling;
    private bool ToolisDigging;
    private bool ToolisWalking;

    [SerializeField] private Tool[] ToolBeltInventory; //일단은 사이즈 4

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
        ToolisDigging = Player.GetComponent<PlayerManager>().isDigging;
        ToolisDrilling = Player.GetComponent<PlayerManager>().isDrilling;
        ToolisWalking = Player.GetComponent<PlayerManager>().isWalking;
        UpdateAnimation();
    }

    void LateUpdate()
    {
        ChangeSkins();
    }

    public int CheckToolBelt(int index)
    {
        curToolSet = ToolBeltInventory[index].curTool;
        return curToolSet;
    }

    private void CheckCurrentToolSkin()
    {
        
    }

    

    private void ChangeSkins()
    {
        if(sp.sprite.name.Contains("MainShovel"))
        {
            string spriteName = sp.sprite.name;
            spriteName = spriteName.Replace("MainShovel_","");
            int spriteNr = int.Parse(spriteName);

            sp.sprite = shovelSkins[skinNr].sprites[spriteNr];
        }
        else
        {
            
        }
        //if Drill
    }


    private void UpdateAnimation()
    {
        anim.SetBool("isDrilling", ToolisDrilling);
        anim.SetBool("isDigging", ToolisDigging);
        anim.SetBool("isWalking", ToolisWalking);
    }


    [System.Serializable] // inspetor에서 보이게 하는 기능
    public struct Skins
    {
        public Sprite[] sprites;
    }
}