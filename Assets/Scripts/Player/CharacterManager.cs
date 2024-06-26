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
    [SerializeField] private PlayerManager playerManager;
    public Transform respawnPos;
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

    [SerializeField] private int[] characterUnlocked;

    SpriteRenderer sp;
    // Start is called before the first frame update
    void Start()
    {
        sp = Player.GetComponent<SpriteRenderer>();
        playerManager = Player.GetComponent<PlayerManager>();
        characterUnlocked = new int[7];
        for(int i = 0; i < characterUnlocked.Length; i++)
        {
            characterUnlocked[i] = 0;
        }
    }

    // Update is called once per frame

    private void Update()
    {
        getGameManagerInfo();
    }

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
        if(gameManager.ArmyTrenchFound)
        {
            characterUnlocked[1] = 1;
        }
        if(gameManager.CrusadeFound)
        {
            characterUnlocked[2] = 2;
        }
        if(gameManager.CatacombFound)
        {
            characterUnlocked[3] = 3;
        }
        if (gameManager.UndergroundTribeFound)
        {
            characterUnlocked[4] = 4;
        }
        if (gameManager.AtlantisFound)
        {
            characterUnlocked[5] = 5;
        }
        if(gameManager.LostWorldFound)
        {
            characterUnlocked[6] = 6;
        }
        if(gameManager.EldoradoFound)
        {
            characterUnlocked[7] = 7;
        }
        // isFound 부분 가져와서 범위 정해주기
        // 0~9 사람, 10~12 군인, 13~15 기사, 16~20 카타콤브, 21~23 두더지, 24 케이브맨
    }

    private int characterRandom() // 게임 플레이로 언락되는 캐릭터 랜덤 돌리기
    {
        int randomIndex = Random.Range(0, 7);
        int skinNr = 0;
        if(characterUnlocked[randomIndex] == 0) // human
        {
            skinNr = Random.Range(0, 10);
        }
        if(characterUnlocked[randomIndex] == 1)// army
        {
            skinNr = Random.Range(10, 13);
        }
        if(characterUnlocked[randomIndex] == 2) //crusade
        {
            skinNr = Random.Range(13, 16);
        }
        if(characterUnlocked[randomIndex] == 3) //catacomb
        {
            skinNr = Random.Range(16, 21);
        }
        if(characterUnlocked[randomIndex] == 4) //mole city
        {
            skinNr = Random.Range(21, 24);
        }
        if(characterUnlocked[randomIndex] == 5) //atlantis
        {
            skinNr = 0;
        }
        if(characterUnlocked[randomIndex] == 6) //lostworld
        {
            skinNr = 24;
        }
        if(characterUnlocked[randomIndex] == 7) //eldorado
        {
            skinNr = 0;
        }
        return skinNr;
    }
    public bool resetCharacter()
    {
        if(gameStart == true)//start of game search between Ids 0 ~ 4
        {
            skinNr = characterRandom();
            if(skinNr == prevNum)
            {
                while(skinNr == prevNum)
                {
                    skinNr = characterRandom();
                }
                curNum = skinNr;
            }
            else
                curNum = skinNr;
        }

        prevNum = curNum;
        //걷게 하기
        Player.GetComponent<PlayerManager>().Dead = false;
        GroundDictionary.GetComponent<GroundDictionary>().MapReset();
        Player.transform.position = respawnPos.position;//new Vector3(5.0f, 5.0f, 0);

        return true;
    }

    void Respawn()
    {
        playerManager.curCharacter = CharactersSO[skinNr];

        if (sp.sprite.name.Contains("MainIdle"))
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
