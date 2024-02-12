using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    public int skinNr;
    public Skins[] IdleSkins;
    public Skins[] DiggingSkins;
    public Skins[] WalkingSkins;
    public Skins[] DrillingSkins;

    SpriteRenderer sp;
    // Start is called before the first frame update
    void Start()
    {
        sp = Player.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void LateUpdate()
    {
        if(Player.GetComponent<PlayerMovement>().Dead == true)
        {
            skinNr = Random.Range(0,3);
            Player.GetComponent<PlayerMovement>().Dead = false;
        }

        Respawn();
    }

    void Respawn()
    {
        if(sp.sprite.name.Contains("MainIdle"))
        {
            string spriteName = sp.sprite.name;
            spriteName = spriteName.Replace("MainIdle_","");
            int spriteNr = int.Parse(spriteName);

            sp.sprite = IdleSkins[skinNr].sprites[spriteNr];
        }

        if(sp.sprite.name.Contains("MainDigging"))
        {
            string spriteName = sp.sprite.name;
            spriteName = spriteName.Replace("MainDigging_","");
            int spriteNr = int.Parse(spriteName);

            sp.sprite = DiggingSkins[skinNr].sprites[spriteNr];
        }

        if(sp.sprite.name.Contains("MainWalking"))
        {
            string spriteName = sp.sprite.name;
            spriteName = spriteName.Replace("MainWalking_","");
            int spriteNr = int.Parse(spriteName);

            sp.sprite = WalkingSkins[skinNr].sprites[spriteNr];
        }

        if(sp.sprite.name.Contains("MainDrilling"))
        {
            string spriteName = sp.sprite.name;
            spriteName = spriteName.Replace("MainDrilling_","");
            int spriteNr = int.Parse(spriteName);

            sp.sprite = DrillingSkins[skinNr].sprites[spriteNr];
        }

    }

    [System.Serializable] // inspetor에서 보이게 하는 기능
    public struct Skins
    {
        public Sprite[] sprites;
    }
}
