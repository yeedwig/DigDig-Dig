using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public int Chapter;
    public int index;

    public GameObject textBubble;
    public Text dialogueText;

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

        Collider2D  playerChecked= Physics2D.OverlapCircle(transform.position, 2.0f, LayerMask.GetMask("Player"));

        if (playerChecked != null && index < Dialogues[Chapter].Dialogues.Length)
        {
            dialogueText.text = Dialogues[Chapter].Dialogues[index].ToString();
            textBubble.SetActive(true);
            //Debug.Log(Dialogues[Chapter].Dialogues[index].ToString());

        }
        if (index >= Dialogues[Chapter].Dialogues.Length)
        {
            index = 1;
        }
        if(playerChecked == null)
        {
            textBubble.SetActive(false);
            index = 0;
        }

    }
}
