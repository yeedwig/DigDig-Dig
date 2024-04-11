using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    public int skinNr;
    public int curNum;
    public int prevNum;

    public GameManager gameManager;

    public bool gameStart = true;
    /*
    public Skins[] IdleSkins;
    public Skins[] DiggingSkins;
    public Skins[] WalkingSkins;
    public Skins[] DrillingSkins;
    public Skins[] ClimbingSkins;
    */

    public CharacterSO[] CharactersSO;

    public GameObject GroundDictionary;

    SpriteRenderer sp;
    // Start is called before the first frame update
    void Start()
    {
        sp = Player.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame


    void LateUpdate()
    {
        /*
        if(Player.GetComponent<PlayerManager>().Dead == true)
        {
            skinNr = Random.Range(0,3);
            Player.GetComponent<PlayerManager>().Dead = false;
            GroundDictionary.GetComponent<GroundDictionary>().MapReset();
            Player.transform.position = new Vector3(1.0f, 5.0f, 0);
        }*/
        Respawn();

    }

    public void getGameManagerInfo()
    {
        // isFound 부분 가져와서 범위 정해주기
    }
    public bool resetCharacter()
    {
        if(gameStart == true)//start of game search between Ids 0 ~ 4
        {
            skinNr = Random.Range(0, 13);
            if(skinNr == prevNum)
            {
                while(skinNr == prevNum)
                {
                    skinNr = Random.Range(0, 13);
                }
                curNum = skinNr;
            }
            else
                curNum = skinNr;
        }

        prevNum = curNum;
        Player.GetComponent<PlayerManager>().Dead = false;
        GroundDictionary.GetComponent<GroundDictionary>().MapReset();
        Player.transform.position = new Vector3(1.5f, 5.0f, 0);

        return true;
    }

    void Respawn()
    {
        
        if(sp.sprite.name.Contains("MainIdle"))
        {
            string spriteName = sp.sprite.name;
            spriteName = spriteName.Replace("MainIdle_","");
            int spriteNr = int.Parse(spriteName);

            //sp.sprite = IdleSkins[skinNr].sprites[spriteNr];
            sp.sprite = CharactersSO[skinNr].IdleSkins[spriteNr];
        }

        if(sp.sprite.name.Contains("MainDigging"))
        {
            string spriteName = sp.sprite.name;
            spriteName = spriteName.Replace("MainDigging_","");
            int spriteNr = int.Parse(spriteName);

            //sp.sprite = DiggingSkins[skinNr].sprites[spriteNr];
            sp.sprite = CharactersSO[skinNr].DiggingSkins[spriteNr];
        }

        if(sp.sprite.name.Contains("MainWalking"))
        {
            string spriteName = sp.sprite.name;
            spriteName = spriteName.Replace("MainWalking_","");
            int spriteNr = int.Parse(spriteName);

            //sp.sprite = WalkingSkins[skinNr].sprites[spriteNr];
            sp.sprite = CharactersSO[skinNr].WalkingSkins[spriteNr];
        }

        if(sp.sprite.name.Contains("MainDrilling"))
        {
            string spriteName = sp.sprite.name;
            spriteName = spriteName.Replace("MainDrilling_","");
            int spriteNr = int.Parse(spriteName);

            //sp.sprite = DrillingSkins[skinNr].sprites[spriteNr];
            sp.sprite = CharactersSO[skinNr].DrillingSkins[spriteNr];
        }

        if(sp.sprite.name.Contains("MainClimbing"))
        {
            string spriteName = sp.sprite.name;
            spriteName = spriteName.Replace("MainClimbing_","");
            int spriteNr = int.Parse(spriteName);

            //sp.sprite = ClimbingSkins[skinNr].sprites[spriteNr];
            sp.sprite = CharactersSO[skinNr].ClimbingSkins[spriteNr];
        }

    }
    /*
    [System.Serializable] // inspetor에서 보이게 하는 기능
    public struct Skins
    {
        public Sprite[] sprites;
    }
    */
}
