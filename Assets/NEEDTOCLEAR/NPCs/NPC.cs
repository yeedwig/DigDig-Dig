using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public int Chapter;
    public int index;
    public GameObject Player;
    public GameObject textBubble;
    public Text dialogueText;

    private bool facingLeft = true;
    public bool canFlip = true;

    public List<Dialogue> Dialogues;
    // Start is called before the first frame update
    void Start()
    {
        Chapter = 0;
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(canFlip)
        {
            Flip();
        }
        

        Collider2D  playerChecked= Physics2D.OverlapCircle(transform.position, 2.0f, LayerMask.GetMask("Player"));

        if (playerChecked != null && index < Dialogues[Chapter].Dialogues.Length)
        {
            dialogueText.text = Dialogues[Chapter].Dialogues[index].ToString();
            textBubble.SetActive(true);
                //Debug.Log(Dialogues[Chapter].Dialogues[index].ToString());

        }
        //여기 잠시만 주석 처리 함
        /*
        if (index >= Dialogues[Chapter].Dialogues.Length)
        {
            index = 0;
        }*/
       if (playerChecked == null)
       {
            textBubble.SetActive(false);
            index = 0;
       }

    }

    public void ChangeChapter()
    {
        Chapter ++;
    }

    private void Flip()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        if (facingLeft == true && this.transform.position.x - Player.transform.position.x < 0.0f)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
            facingLeft = false;
        }
        if(facingLeft == false && this.transform.position.x - Player.transform.position.x >= 0.0f)
        {
            this.GetComponent<SpriteRenderer>().flipX = false;//transform.Rotate(0.0f, 180.0f, 0.0f);
            facingLeft = true;
        }
    }


}
